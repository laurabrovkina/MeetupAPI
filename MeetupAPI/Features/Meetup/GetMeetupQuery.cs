using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using ErrorHandling.Exceptions;
using Meetup.Contracts.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Features.Meetup;

public record GetMeetupQuery(string Name) : IRequest<MeetupDetailsDto>;

public class GetMeetupQueryHandler : IRequestHandler<GetMeetupQuery, MeetupDetailsDto>
{
    private readonly MeetupContext _context;
    private readonly IMapper _mapper;

    public GetMeetupQueryHandler(MeetupContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MeetupDetailsDto> Handle(GetMeetupQuery request, CancellationToken cancellationToken)
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