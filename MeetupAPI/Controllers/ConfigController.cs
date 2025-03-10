using System.Threading.Tasks;
using Features.Config;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("config")]
public class ConfigController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConfigController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpOptions("reload")]
    public async Task<ActionResult> Reload()
    {
        var success = await _mediator.Send(new ReloadConfigCommand());
        return success ? Ok() : StatusCode(500);
    }
}