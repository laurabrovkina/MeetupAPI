using System.Linq;
using Entities;
using FluentValidation;
using MeetupAPI.Models;

namespace Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
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
