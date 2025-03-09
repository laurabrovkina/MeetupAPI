using System.Threading.Tasks;
using Features.Account.Login;
using Features.Account.Register;
using Features.Account.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterCommand command)
    {
        var email = await _mediator.Send(command);
        return CreatedAtAction(nameof(Login), new { email });
    }

    [HttpPut("edit")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<ActionResult> Edit([FromBody] UpdateUserCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}