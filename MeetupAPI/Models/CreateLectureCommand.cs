using MediatR;
using MeetupAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAPI.Models
{
    public class CreateLectureCommand : IRequest<Lecture>
    {
        [FromBody]
        public string Author { get; set; }
        [FromBody]
        public string Topic { get; set; }
        [FromBody]
        public string Description { get; set; }
        [FromRoute]
        public string MeetupName { get; set; }
    }
}
