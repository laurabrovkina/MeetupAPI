using System.Linq;
using FluentValidation;
using MeetupAPI.Entities;
using MeetupAPI.Models;

namespace MeetupAPI.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator(MeetupContext meetupContext)
    {
        RuleFor(x => x.Email).Custom((value, context) =>
        {
            var userAlreadyExists = meetupContext.Users.Any(user => user.Email == value);
            if (userAlreadyExists)
            {
                context.AddFailure("Email","This email address is taken");
            }
        });
    }
}
