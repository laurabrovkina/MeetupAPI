using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using ErrorHandling.Exceptions;
using Mediator;
using Meetup.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace Features.Meetup;

/// <summary>
/// GetMeetupRequest
/// </summary>
/// <param name="Name"></param>
public record GetMeetupRequest(string Name) : IRequest<MeetupDetailsDto>;

/// <summary>
/// Using nuget package called Mediator.SourceGenerator and Mediator.Abstractions
/// </summary>
public class FasterMediator : IRequestHandler<GetMeetupRequest, MeetupDetailsDto>
{
    private readonly MeetupContext _context;
    private readonly IMapper _mapper;

    public FasterMediator(MeetupContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<MeetupDetailsDto> Handle(GetMeetupRequest request, CancellationToken cancellationToken)
    {
        var meetup = await _context.Meetups
            .Include(m => m.Location)
            .Include(m => m.Lectures)
            .FirstOrDefaultAsync(m => m.Name.Replace(" ", "-").ToLower() == request.Name.ToLower(),
                cancellationToken);

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound,
                $"Meetup with name {request.Name} has not been found");
        }

        return _mapper.Map<MeetupDetailsDto>(meetup);
    }
}