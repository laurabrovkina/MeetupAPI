using FluentValidation;
using MeetupAPI.Lectures;

namespace MeetupAPI.Validators
{
    public class DeleteLectureByIdValidator : AbstractValidator<DeleteLectureByIdCommand>
    {
        public DeleteLectureByIdValidator()
        {
            RuleFor(x => x.MeetupName)
                .MinimumLength(5)
                .WithMessage("The Meetup Name can't be less than 5 characters.");

            RuleFor(x => x.MeetupId)
                .NotNull()
                .WithMessage("Id can't be null.");
        }
    }
}
