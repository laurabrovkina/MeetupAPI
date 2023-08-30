using FluentValidation;
using MeetupAPI.Models;

namespace MeetupAPI.Validators
{
    public class CreateLectureValidator : AbstractValidator<CreateLectureCommand>
    {
        public CreateLectureValidator()
        {
            RuleFor(x => x.Author)
                .MinimumLength(5)
                .WithMessage("The author can't be less than 5 characters.");
            RuleFor(x => x.Topic)
                .MinimumLength(5)
                .WithMessage("The topic can't be less than 5 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(200)
                .WithMessage("The description max length is 200 characters.");
        }
    }
}
