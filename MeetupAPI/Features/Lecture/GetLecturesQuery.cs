using System.Collections.Generic;
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

public record GetLecturesQuery(string MeetupName) : IRequest<List<LectureDto>>;

public class GetLecturesQueryHandler : IRequestHandler<GetLecturesQuery, List<LectureDto>>
{
    private readonly MeetupContext _context;
    private readonly IMapper _mapper;

    public GetLecturesQueryHandler(MeetupContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<LectureDto>> Handle(GetLecturesQuery request, CancellationToken cancellationToken)
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

        return _mapper.Map<List<LectureDto>>(meetup.Lectures);
    }
}