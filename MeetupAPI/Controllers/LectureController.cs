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
        public async Task<ActionResult> Delete(DeleteLectureByNameCommand name)
        {
            await _mediator.Send(name);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(DeleteLectureByIdCommand request)
        {          
            await _mediator.Send(request);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> Get(GetLectureCommand request)
        {
            var lectures = await _mediator.Send(request);

            return Ok(lectures);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateLectureCommand request)
        {
            await _mediator.Send(request);

            return Created($"api/meetup/{request.MeetupName}", null);
        }
    }
}
