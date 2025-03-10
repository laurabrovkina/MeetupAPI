using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using ErrorHandling.Exceptions;
using Meetup.Contracts.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Features.Lecture;

public record CreateLectureCommand(string MeetupName, LectureDto Lecture) : IRequest;

public class CreateLectureCommandHandler : IRequestHandler<CreateLectureCommand>
{
    private readonly MeetupContext _context;
    private readonly IMapper _mapper;

    public CreateLectureCommandHandler(MeetupContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(CreateLectureCommand request, CancellationToken cancellationToken)
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

        var lecture = _mapper.Map<Entities.Lecture>(request.Lecture);
        meetup.Lectures.Add(lecture);
        await _context.SaveChangesAsync(cancellationToken);
    }
}