using System.Linq;
using Entities;
using FluentValidation;
using Meetup.Contracts.Models;

namespace Validators;

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
