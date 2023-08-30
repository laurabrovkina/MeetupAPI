using FluentValidation;
using MeetupAPI.Lectures;

namespace MeetupAPI.Validators
{
    public class DeleteLectureByNameValidator : AbstractValidator<DeleteLectureByNameCommand>
    {
        public DeleteLectureByNameValidator()
        {
            RuleFor(x => x.MeetupName)
                .MinimumLength(5)
                .WithMessage("The Meetup Name can't be less than 5 characters.");
        }
    }
}
