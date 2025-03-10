using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Meetup.Contracts.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Features.Meetup;

public record GetMeetupsQuery(MeetupQuery Query) : IRequest<PagedResult<MeetupDetailsDto>>;

public class GetMeetupsQueryHandler : IRequestHandler<GetMeetupsQuery, PagedResult<MeetupDetailsDto>>
{
    private readonly MeetupContext _context;
    private readonly IMapper _mapper;

    public GetMeetupsQueryHandler(MeetupContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<MeetupDetailsDto>> Handle(GetMeetupsQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = _context.Meetups
            .Include(m => m.Location)
            .Where(m => request.Query.SearchPhrase == null ||
                       m.Organizer.ToLower().Contains(request.Query.SearchPhrase.ToLower()) ||
                       m.Name.Contains(request.Query.SearchPhrase.ToLower()));

        if (!string.IsNullOrEmpty(request.Query.SortBy))
        {
            var propertySelectors = new Dictionary<string, Expression<Func<Entities.Meetup, object>>>
            {
                { nameof(Entities.Meetup.Name), meetup => meetup.Name },
                { nameof(Entities.Meetup.Date), meetup => meetup.Date },
                { nameof(Entities.Meetup.Organizer), meetup => meetup.Organizer }
            };

            var propertySelector = propertySelectors[request.Query.SortBy];

            baseQuery = request.Query.SortDirection == SortDirection.ASC
                ? baseQuery.OrderBy(m => m.Date)
                : baseQuery.OrderByDescending(m => m.Name);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var meetups = await baseQuery
            .Skip(request.Query.PageSize * (request.Query.PageNumber - 1))
            .Take(request.Query.PageSize)
            .ToListAsync(cancellationToken);

        var meetupDtos = _mapper.Map<List<MeetupDetailsDto>>(meetups);

        return new PagedResult<MeetupDetailsDto>(meetupDtos, totalCount, request.Query.PageNumber, request.Query.PageSize);
    }
}