using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetupAPI.Entities;
using MeetupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Lectures
{
    public class CreateLectureHandler : IRequestHandler<CreateLectureCommand, Lecture>
    {
        private readonly MeetupContext _meetupContext;
        public CreateLectureHandler(MeetupContext meetupContext)
        {
            _meetupContext = meetupContext;
        }

        public async Task<Lecture> Handle(CreateLectureCommand request, CancellationToken cancellationToken)
        {
            var meetup = _meetupContext.Meetups
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == request.MeetupName.ToLower());

            if (meetup == null)
            {
                throw new System.Exception();
            }

            var lecture = new Lecture
            {
                Author = request.Lecture.Author,
                Topic = request.Lecture.Topic,
                Description = request.Lecture.Description
            };

            meetup.Lectures.Add(lecture);
            _meetupContext.SaveChanges();

            return await Task.FromResult(lecture);
        }
    }
}