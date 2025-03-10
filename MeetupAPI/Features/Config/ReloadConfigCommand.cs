using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Features.Config;

public record ReloadConfigCommand() : IRequest<bool>;

public class ReloadConfigCommandHandler : IRequestHandler<ReloadConfigCommand, bool>
{
    private readonly IConfiguration _configuration;

    public ReloadConfigCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<bool> Handle(ReloadConfigCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ((IConfigurationRoot)_configuration).Reload();
            return Task.FromResult(true);
        }
        catch (Exception)
        {
            return Task.FromResult(false);
        }
    }
}