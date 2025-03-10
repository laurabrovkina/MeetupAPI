using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Authorization;

/// <summary>
/// MeetupResourceOperationHandler
/// </summary>
public class MeetupResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, Entities.Meetup>
{
    /// <summary>
    /// HandleRequirementAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Entities.Meetup resource)
    {
        if (requirement.OperationType == OperationType.Create || requirement.OperationType == OperationType.Read)
        {
            context.Succeed(requirement);
        }

        var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && resource.CreatedById == int.Parse(userId))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
