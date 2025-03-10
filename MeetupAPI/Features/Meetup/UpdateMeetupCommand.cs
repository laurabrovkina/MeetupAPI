using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authorization;
using Entities;
using ErrorHandling.Exceptions;
using Meetup.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Features.Meetup;

public record UpdateMeetupCommand(string Name, MeetupDto Meetup, ClaimsPrincipal User) : IRequest;

public class UpdateMeetupCommandHandler : IRequestHandler<UpdateMeetupCommand>
{
    private readonly MeetupContext _context;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMeetupApiMetrics _metrics;

    public UpdateMeetupCommandHandler(
        MeetupContext context,
        IAuthorizationService authorizationService,
        IMeetupApiMetrics metrics)
    {
        _context = context;
        _authorizationService = authorizationService;
        _metrics = metrics;
    }

    public async Task Handle(UpdateMeetupCommand request, CancellationToken cancellationToken)
    {
        using var _ = _metrics.MeasureRequestDuration();

        var meetup = await _context.Meetups
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
            new ResourceOperationRequirement(OperationType.Update));

        if (!authorizationResult.Succeeded)
        {
            throw new ApiResponseException(HttpStatusCode.Forbidden, "Forbidden");
        }

        meetup.Name = request.Meetup.Name;
        meetup.Organizer = request.Meetup.Organizer;
        meetup.Date = request.Meetup.Date;
        meetup.IsPrivate = request.Meetup.IsPrivate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}