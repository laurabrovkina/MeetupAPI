using System.Linq;
using System.Net;
using Entities;
using ErrorHandling.Exceptions;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Lecture = MeetupAPI.Models.Lecture;

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

        var lectures = new LectureResponse
        {
            Lectures = meetup.Lectures.Select(x => new Lecture
            {
                Author = x.Author,
                Topic = x.Topic,
                Description = x.Description
            }).ToList(),
            MeetupName = meetup.Name,
            MeetupOrganizer = meetup.Organizer
        };

        return Ok(lectures);
    }

    [HttpPost]
    public ActionResult Post(string meetupName, [FromBody] LectureRequest request)
    {
        var meetup = _meetupContext.Meetups
            .Include(m => m.Lectures)
            .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"Meetup with name {meetupName} has not been found");
        }

        var lecture = new Entities.Lecture
        {
            Author = request.Author,
            Topic = request.Topic,
            Description = request.Description,
            Meetup = meetup,
            MeetupId = meetup.Id
        };
        
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
