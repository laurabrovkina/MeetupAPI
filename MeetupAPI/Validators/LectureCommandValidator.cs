using FluentValidation;
using MeetupAPI.Lectures;

namespace MeetupAPI.Validators
{
    public class CreateLectureValidator : AbstractValidator<CreateLectureCommand>
    {
        public CreateLectureValidator()
        {
            RuleFor(x => x.Author).NotEmpty().MinimumLength(5);
            RuleFor(x => x.Topic).NotEmpty().MinimumLength(5);
            RuleFor(x => x.Description).MaximumLength(200);
        }
    }
}
