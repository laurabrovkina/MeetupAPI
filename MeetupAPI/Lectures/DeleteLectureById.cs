using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetupAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Lectures
{
    public record DeleteLectureByIdCommand : IRequest<Unit>
    {
        [FromRoute]
        public string MeetupName { get; set; }
        [FromRoute]
        public int MeetupId { get; set; }
    }

    public class DeleteLectureByIdHandler : IRequestHandler<DeleteLectureByIdCommand, Unit>
    {
        private readonly MeetupContext _meetupContext;

        public DeleteLectureByIdHandler(MeetupContext meetupContext)
        {
            _meetupContext = meetupContext;
        }

        public async Task<Unit> Handle(DeleteLectureByIdCommand request, CancellationToken cancellationToken)
        {
            var meetup = _meetupContext.Meetups
                .Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == request.MeetupName.ToLower()) 
                    ?? throw new System.Exception();
            
            var lecture = meetup.Lectures.FirstOrDefault(l => l.Id == request.MeetupId) 
                ?? throw new System.Exception();

            _meetupContext.Lectures.Remove(lecture);
            await _meetupContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}