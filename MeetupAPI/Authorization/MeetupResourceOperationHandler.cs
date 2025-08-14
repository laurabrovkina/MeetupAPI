using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace Authorization;

public class MeetupResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, Meetup>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Meetup resource)
    {
        if (requirement.OperationType == OperationType.Create || requirement.OperationType == OperationType.Read)
        {
            context.Succeed(requirement);
        }

        var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId), "User Id has not found");
        }

        if (resource.CreatedById == int.Parse(userId))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
