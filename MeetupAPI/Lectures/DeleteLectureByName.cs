using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetupAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeetupAPI.Lectures
{
    public record DeleteLectureByNameCommand : IRequest
    {
        public string MeetupName { get; set; }
    }

    public class DeleteLectureByNameHandler : IRequestHandler<DeleteLectureByNameCommand>
    {
        private readonly MeetupContext _meetupContext;
        private readonly ILogger<DeleteLectureByNameHandler> _logger;

        public DeleteLectureByNameHandler(MeetupContext meetupContext)
        {
            _meetupContext = meetupContext;
        }

        public async Task Handle(DeleteLectureByNameCommand request, CancellationToken cancellationToken)
        {
            var meetup = _meetupContext.Meetups
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == request.MeetupName.ToLower());

            if (meetup == null)
            {
                throw new System.Exception();
            }

            _logger.LogWarning($"The lectures for meetup {meetup.Name} have been removed.");

            _meetupContext.Lectures.RemoveRange(meetup.Lectures);
            _meetupContext.SaveChanges();
        }
    }
}