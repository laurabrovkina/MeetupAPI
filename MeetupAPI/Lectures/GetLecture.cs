using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MeetupAPI.Entities;
using MeetupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Lectures
{
    public record GetLectureCommand() : IRequest<List<CreateLectureCommand>>
    {
        public string MeetupName { get; set; }
    }

    public class GetLectureHandler : IRequestHandler<GetLectureCommand, List<CreateLectureCommand>>
    {
        private readonly MeetupContext _meetupContext;
        private readonly IMapper _mapper;

        public GetLectureHandler(MeetupContext meetupContext, IMapper mapper)
        {
            _meetupContext = meetupContext;
            _mapper = mapper;
        }

        public async Task<List<CreateLectureCommand>> Handle(GetLectureCommand request, CancellationToken cancellationToken)
        {
            var meetup = _meetupContext.Meetups
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == request.MeetupName.ToLower());

            if (meetup == null)
            {
                throw new System.Exception();
            }

            var lectures = _mapper.Map<List<CreateLectureCommand>>(meetup.Lectures);

            return await Task.FromResult(lectures);
        }
    }
}