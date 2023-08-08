using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetupAPI.Entities;
using MeetupAPI.Validators;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Lectures
{
    public record CreateLectureCommand() : IRequest<Lecture>
    {
        public string Author { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string Filter { get; set; }
    }

    public class CreateLectureHandler : IRequestHandler<CreateLectureCommand, Lecture>
    {
        private readonly MeetupContext _meetupContext;
        private readonly CreateLectureValidator _validator = new();
        public CreateLectureHandler(MeetupContext meetupContext)
        {
            _meetupContext = meetupContext;
        }

        public async Task<Lecture> Handle(CreateLectureCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new System.Exception();
            }

            var meetup = _meetupContext.Meetups
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == request.Filter.ToLower());

            if (meetup == null)
            {
                throw new System.Exception();
            }

            var lecture = new Lecture
            {
                Author = request.Author,
                Topic = request.Topic,
                Description = request.Description
            };

            meetup.Lectures.Add(lecture);
            _meetupContext.SaveChanges();

            return await Task.FromResult(lecture);
        }
    }
}