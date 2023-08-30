using MediatR;
using MeetupAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAPI.Models
{
    public class CreateLectureCommand : IRequest<Lecture>
    {
        [FromBody]
        public LectureDto Lecture { get; set; }

        [FromRoute]
        public string MeetupName { get; set; }
    }

    public class LectureDto
    {
        public string Author { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
    }
}
