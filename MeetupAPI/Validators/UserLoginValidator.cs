using FluentValidation;
using MeetupAPI.Models;

namespace Validators;

public class UserLoginValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).MinimumLength(6);
        RuleFor(x => x.Password).Equal(x => x.ConfirmPassword);
    }
}
