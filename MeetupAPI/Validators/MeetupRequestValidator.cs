using FluentValidation;
using MeetupAPI.Models;

namespace Validators;

public class MeetupRequestValidator : AbstractValidator<MeetupRequest>
{
    public MeetupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(3)
            .WithMessage("Name should be at least 3 characters long");

        RuleFor(x => x.Organizer)
            .NotEmpty()
            .WithMessage("Organizer is required");
    }
}