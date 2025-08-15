using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization;
using Entities;
using ErrorHandling;
using ErrorHandling.Exceptions;
using Facet.Extensions;
using Filters;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetupResponse = Mappings.MeetupResponse;

namespace Controllers;

[Route("api/meetup")]
[Authorize]
[ServiceFilter(typeof(TimeTrackFilter))]
public class MeetupController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly MeetupContext _meetupContext;
    private readonly IMeetupApiMetrics _metrics;

    public MeetupController(MeetupContext meetupContext,
        IAuthorizationService authorizationService,
        IMeetupApiMetrics metrics)
    {
        _meetupContext = meetupContext;
        _authorizationService = authorizationService;
        _metrics = metrics;
    }

    [HttpGet]
    //[NationalityFilter("German, French")]
    [AllowAnonymous]
    public ActionResult<PagedResult<MeetupResponse>> GetAll([FromQuery] MeetupQuery query)
    {
        using var _ = _metrics.MeasureRequestDuration();

        if (!ModelState.IsValid)
        {
            ErrorMessages.BadRequestMessage(query, ModelState);
        }

        var baseQuery = _meetupContext.Meetups
            .Include(m => m.Location)
            .Where(m => query.SearchPhrase == null ||
                        m.Organizer.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                        m.Name.Contains(query.SearchPhrase.ToLower()));

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var propertySelectors = new Dictionary<string, Expression<Func<Meetup, object>>>
            {
                { nameof(Meetup.Name), meetup => meetup.Name },
                { nameof(Meetup.Date), meetup => meetup.Date },
                { nameof(Meetup.Organizer), meetup => meetup.Organizer }
            };

            var propertySelector = propertySelectors[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC
                ? baseQuery.OrderBy(m => m.Date)
                : baseQuery.OrderByDescending(m => m.Name);
        }

        var meetups = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        var totalCount = baseQuery.Count();

        var meetupDtos = meetups.Select(x => x.ToFacet<Meetup, MeetupResponse>()).ToList();
        var result = new PagedResult<MeetupResponse>(meetupDtos, totalCount, query.PageNumber, query.PageSize);

        return Ok(result);
    }

    [HttpGet("{name}")]
    //[NationalityFilter("English")]
    // Authorisation policy that checks if the user has a claim nationality
    //[Authorize(Policy = "HasNationality")]
    //[Authorize(Policy = "AtLeast18")]
    public ActionResult<MeetupResponse> Get(string name)
    {
        using var _ = _metrics.MeasureRequestDuration();

        var meetup = _meetupContext.Meetups
            .Include(m => m.Location)
            .Include(m => m.Lectures)
            .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Meetup with name {name} has not been found");
        }

        var meetupDto = meetup.ToFacet<Meetup, MeetupResponse>();
        return Ok(meetupDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<ActionResult> Post([FromBody] MeetupRequest model)
    {
        using var _ = _metrics.MeasureRequestDuration();
        _metrics.IncreaseMeetupRequestCount();

        if (!ModelState.IsValid)
        {
            ErrorMessages.BadRequestMessage(model, ModelState);
        }

        var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var meetup = Meetup.Create(model, userId);

        await _meetupContext.Meetups.AddAsync(meetup);
        // Interceptor publishes the event
        await _meetupContext.SaveChangesAsync();

        var key = meetup.Name.Replace(" ", "-").ToLower();
        return Created("api/meetup/" + key, null);
    }

    [HttpPut("{name}")]
    public ActionResult Put(string name, [FromBody] MeetupRequest model)
    {
        using var _ = _metrics.MeasureRequestDuration();

        var meetup = _meetupContext.Meetups
            .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Meetup with name {name} has not been found");
        }

        var authorizationResult = _authorizationService.AuthorizeAsync(User, meetup, new ResourceOperationRequirement(OperationType.Update)).Result;

        if (!authorizationResult.Succeeded)
        {
            throw new ApiResponseException(HttpStatusCode.Forbidden, "Forbidden");
        }

        if (!ModelState.IsValid)
        {
            ErrorMessages.BadRequestMessage(model, ModelState);
        }

        meetup.Name = model.Name;
        meetup.Organizer = model.Organizer;
        meetup.Date = model.Date;
        meetup.IsPrivate = model.IsPrivate;

        _meetupContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{name}")]
    public ActionResult Delete(string name)
    {
        using var _ = _metrics.MeasureRequestDuration();

        var meetup = _meetupContext.Meetups
            .Include(m => m.Location)
            .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Meetup with name {name} has not been found");
        }

        var authorizationResult = _authorizationService.AuthorizeAsync(User, meetup, new ResourceOperationRequirement(OperationType.Delete)).Result;

        if (!authorizationResult.Succeeded)
        {
            throw new ApiResponseException(HttpStatusCode.Forbidden, "Forbidden");
        }

        _meetupContext.Remove(meetup);
        _meetupContext.SaveChanges();

        return NoContent();
    }
}