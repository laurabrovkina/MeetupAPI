using System.Threading.Tasks;
using Features.File;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("file")]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "name" })]
    [HttpGet]
    public async Task<ActionResult> GetFile(string name)
    {
        var result = await _mediator.Send(new GetFileQuery(name));

        if (result == null)
        {
            return NotFound();
        }

        return File(result.FileContent, result.ContentType, result.FileName);
    }
}
