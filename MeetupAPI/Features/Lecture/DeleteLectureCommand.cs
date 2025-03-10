using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using ErrorHandling.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Features.Lecture;

public record DeleteLectureCommand(string MeetupName, int LectureId) : IRequest;

public class DeleteLectureCommandHandler : IRequestHandler<DeleteLectureCommand>
{
    private readonly MeetupContext _context;

    public DeleteLectureCommandHandler(MeetupContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLectureCommand request, CancellationToken cancellationToken)
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

        var lecture = meetup.Lectures.FirstOrDefault(l => l.Id == request.LectureId);

        if (lecture == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound,
                $"Lecture with id {request.LectureId} has not been found");
        }

        _context.Lectures.Remove(lecture);
        await _context.SaveChangesAsync(cancellationToken);
    }
}