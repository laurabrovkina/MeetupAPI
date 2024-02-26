using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeetupAPI.Authorization;

public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    private readonly ILogger<MinimumAgeHandler> _logger;

    public MinimumAgeHandler(ILogger<MinimumAgeHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        // find the claim we are interested in
        var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
        // Suggestion: we could get the value from the database

        var userName = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
        _logger.LogInformation($"Handling minimum age requirement for: {userName}. [dateOfBirth: {dateOfBirth}]");

        // check the requirement
        if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
        {
            _logger.LogInformation("Access granted");
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogInformation("Access denied");
        }

        return Task.CompletedTask;
    }
}
