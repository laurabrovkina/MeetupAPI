using System.Linq;
using FluentValidation;
using MeetupAPI.Entities;
using MeetupAPI.Models;

namespace MeetupAPI.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator(MeetupContext meetupContext)
    {
        RuleFor(x => x.RoleId).Custom((value, context) =>
        {
            var allowedRoleIds = meetupContext.Roles.Select(x => x.Id).ToList();

            if (!allowedRoleIds.Contains(value))
            {
                context.AddFailure("RoleId", $"Role doesn't exist");
            }
        });
        RuleFor(x => x.Email).Custom((value, context) =>
        {
            var userAlreadyExists = meetupContext.Users.Any(user => user.Email == value);
            if (!userAlreadyExists)
            {
                context.AddFailure("Email","This email address doesn't exist");
            }
        });
    }
}
