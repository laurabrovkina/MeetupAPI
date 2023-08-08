using System.Threading.Tasks;
using MediatR;
using MeetupAPI.Lectures;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAPI.Controllers
{
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
            var command = new DeleteLectureByNameCommand
            {
                MeetupName = meetupName
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string meetupName, int id)
        {
            var command = new DeleteLectureByIdCommand
            {
                MeetupName = meetupName,
                MeetupId = id
            };
            
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> Get(string meetupName)
        {
            var command = new GetLectureCommand
            {
                MeetupName = meetupName
            };

            var lectures = await _mediator.Send(command);

            return Ok(lectures);
        }

        [HttpPost]
        public async Task<ActionResult> Post(string meetupName, [FromBody] LectureDto request)
        {
            var command = new CreateLectureCommand
            {
                Author = request.Author,
                Topic = request.Topic,
                Description = request.Description,
                Filter = meetupName
            };

            await _mediator.Send(command);

            return Created($"api/meetup/{meetupName}", null);
        }
    }
}
