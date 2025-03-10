using System.Threading.Tasks;
using Features.Lecture;
using Meetup.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("api/meetup/{meetupName}/lecture")]
public class LectureController : ControllerBase
{
    private readonly IMediator _mediator;

    public LectureController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(string meetupName)
    {
        await _mediator.Send(new DeleteAllLecturesCommand(meetupName));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string meetupName, int id)
    {
        await _mediator.Send(new DeleteLectureCommand(meetupName, id));
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult> Get(string meetupName)
    {
        var lectures = await _mediator.Send(new GetLecturesQuery(meetupName));
        return Ok(lectures);
    }

    [HttpPost]
    public async Task<ActionResult> Post(string meetupName, [FromBody] LectureDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _mediator.Send(new CreateLectureCommand(meetupName, model));
        return Created($"api/meetup/{meetupName}", null);
    }
}
