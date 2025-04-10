using System.Security.Claims;
using System.Threading.Tasks;
using Features.Meetup;
using Meetup.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Filters;

namespace Controllers;

[Route("api/meetup")]
//[Authorize]
[ServiceFilter(typeof(TimeTrackFilter))]
public class MeetupController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRequestHandler<GetMeetupRequest, MeetupDetailsDto> _requestHandler;

    public MeetupController(IMediator mediator,
        IRequestHandler<GetMeetupRequest, MeetupDetailsDto> requestHandler)
    {
        _mediator = mediator;
        _requestHandler = requestHandler;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<MeetupDetailsDto>>> GetAll([FromQuery] MeetupQuery query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(new GetMeetupsQuery(query));
        return Ok(result);
    }

    // [HttpGet("{name}")]
    // public async Task<ActionResult<MeetupDetailsDto>> Get(string name)
    // {
    //     var result = await _mediator.Send(new GetMeetupQuery(name));
    //     return Ok(result);
    // }   
    
    [HttpGet("{name}")]
    public async Task<ActionResult<MeetupDetailsDto>> Get(string name)
    {
        var result = await _requestHandler.Handle(new GetMeetupRequest(name));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<ActionResult> Post([FromBody] MeetupDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var key = await _mediator.Send(new CreateMeetupCommand(model, userId));

        return Created($"api/meetup/{key}", null);
    }

    [HttpPut("{name}")]
    public async Task<ActionResult> Put(string name, [FromBody] MeetupDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _mediator.Send(new UpdateMeetupCommand(name, model, User));
        return NoContent();
    }

    [HttpDelete("{name}")]
    public async Task<ActionResult> Delete(string name)
    {
        await _mediator.Send(new DeleteMeetupCommand(name, User));
        return NoContent();
    }
}