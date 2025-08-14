using System.Linq;
using System.Net;
using Entities;
using ErrorHandling;
using ErrorHandling.Exceptions;
using Facet.Extensions;
using Mappings;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Controllers;

[Route("api/meetup/{meetupName}/lecture")]
public class LectureController : ControllerBase
{
    private readonly MeetupContext _meetupContext;
    private readonly ILogger<LectureController> _logger;

    public LectureController(MeetupContext meetupContext, ILogger<LectureController> logger)
    {
        _meetupContext = meetupContext;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Get(string meetupName)
    {
        var meetup = _meetupContext.Meetups
            .Include(m => m.Lectures)
            .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Meetup with name {meetupName} has not been found");
        }

        var lectures = meetup.Lectures.Select(x => x.ToFacet<Lecture, LectureRequest>());

        return Ok(lectures);
    }

    [HttpPost]
    public ActionResult Post(string meetupName, [FromBody] LectureRequest model)
    {
        if (!ModelState.IsValid)
        {
            ErrorMessages.BadRequestMessage(model, ModelState);
        }
        var meetup = _meetupContext.Meetups
            .Include(m => m.Lectures)
            .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Meetup with name {meetupName} has not been found");
        }

        // this is not set correctly
        // we need to find a way to map request to lecture entity in DB
        var lecture = model.ToFacet<LectureRequest, Lecture>();
        meetup.Lectures.Add(lecture);
        _meetupContext.SaveChanges();

        return Created($"api/meetup/{meetupName}", null);
    }

    [HttpDelete]
    public ActionResult Delete(string meetupName)
    {
        var meetup = _meetupContext.Meetups
            .Include(m => m.Lectures)
            .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Meetup with name {meetupName} has not been found");
        }

        _logger.LogWarning($"The lectures for meetup {meetup.Name} have been removed.");

        _meetupContext.Lectures.RemoveRange(meetup.Lectures);
        _meetupContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string meetupName, int id)
    {
        var meetup = _meetupContext.Meetups
            .Include(m => m.Lectures)
            .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Meetup with name {meetupName} has not been found");
        }

        var lecture = meetup.Lectures.FirstOrDefault(l => l.Id == id);

        if (lecture == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Lecture with name {lecture} has not been found");
        }

        _meetupContext.Lectures.Remove(lecture);
        _meetupContext.SaveChanges();

        return NoContent();
    }
}
