using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using ErrorHandling.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Features.Lecture;

public record DeleteAllLecturesCommand(string MeetupName) : IRequest;

public class DeleteAllLecturesCommandHandler : IRequestHandler<DeleteAllLecturesCommand>
{
    private readonly MeetupContext _context;
    private readonly ILogger<DeleteAllLecturesCommandHandler> _logger;

    public DeleteAllLecturesCommandHandler(
        MeetupContext context,
        ILogger<DeleteAllLecturesCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(DeleteAllLecturesCommand request, CancellationToken cancellationToken)
    {
        var meetup = await _context.Meetups
            .Include(m => m.Lectures)
            .FirstOrDefaultAsync(m => m.Name.Replace(" ", "-").ToLower() == request.MeetupName.ToLower(),
                cancellationToken);

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound,
                $"Meetup with name {request.MeetupName} has not been found");
        }

        _logger.LogWarning($"The lectures for meetup {meetup.Name} have been removed.");

        _context.Lectures.RemoveRange(meetup.Lectures);
        await _context.SaveChangesAsync(cancellationToken);
    }
}