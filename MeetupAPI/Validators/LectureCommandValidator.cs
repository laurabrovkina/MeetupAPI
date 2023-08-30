using FluentValidation;
using MeetupAPI.Models;

namespace MeetupAPI.Validators
{
    public class CreateLectureValidator : AbstractValidator<CreateLectureCommand>
    {
        public CreateLectureValidator()
        {
            RuleFor(x => x.Lecture.Author)
                .MinimumLength(5)
                .WithMessage("The author can't be less than 5 characters.");
            RuleFor(x => x.Lecture.Topic)
                .MinimumLength(5)
                .WithMessage("The topic can't be less than 5 characters.");
            RuleFor(x => x.Lecture.Description)
                .MaximumLength(200)
                .WithMessage("The description max length is 200 characters.");
        }
    }
}
