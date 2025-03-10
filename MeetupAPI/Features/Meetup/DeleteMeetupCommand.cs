using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authorization;
using Entities;
using ErrorHandling.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Features.Meetup;

public record DeleteMeetupCommand(string Name, ClaimsPrincipal User) : IRequest;

public class DeleteMeetupCommandHandler : IRequestHandler<DeleteMeetupCommand>
{
    private readonly MeetupContext _context;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMeetupApiMetrics _metrics;

    public DeleteMeetupCommandHandler(
        MeetupContext context,
        IAuthorizationService authorizationService,
        IMeetupApiMetrics metrics)
    {
        _context = context;
        _authorizationService = authorizationService;
        _metrics = metrics;
    }

    public async Task Handle(DeleteMeetupCommand request, CancellationToken cancellationToken)
    {
        using var _ = _metrics.MeasureRequestDuration();

        var meetup = await _context.Meetups
            .Include(m => m.Location)
            .FirstOrDefaultAsync(m => m.Name.Replace(" ", "-").ToLower() == request.Name.ToLower(),
                cancellationToken);

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound,
                $"Meetup with name {request.Name} has not been found");
        }

        var authorizationResult = await _authorizationService.AuthorizeAsync(
            request.User,
            meetup,
            new ResourceOperationRequirement(OperationType.Delete));

        if (!authorizationResult.Succeeded)
        {
            throw new ApiResponseException(HttpStatusCode.Forbidden, "Forbidden");
        }

        _context.Remove(meetup);
        await _context.SaveChangesAsync(cancellationToken);
    }
}