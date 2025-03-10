using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Meetup.Contracts.Models;
using MediatR;

namespace Features.Meetup;

public record CreateMeetupCommand(MeetupDto Meetup, int UserId) : IRequest<string>;

public class CreateMeetupCommandHandler : IRequestHandler<CreateMeetupCommand, string>
{
    private readonly MeetupContext _context;
    private readonly IMapper _mapper;
    private readonly IMeetupApiMetrics _metrics;

    public CreateMeetupCommandHandler(
        MeetupContext context,
        IMapper mapper,
        IMeetupApiMetrics metrics)
    {
        _context = context;
        _mapper = mapper;
        _metrics = metrics;
    }

    public async Task<string> Handle(CreateMeetupCommand request, CancellationToken cancellationToken)
    {
        using var _ = _metrics.MeasureRequestDuration();
        _metrics.IncreaseMeetupRequestCount();

        var meetup = _mapper.Map<Entities.Meetup>(request.Meetup);
        meetup.CreatedById = request.UserId;

        _context.Meetups.Add(meetup);
        await _context.SaveChangesAsync(cancellationToken);

        return meetup.Name.Replace(" ", "-").ToLower();
    }
}