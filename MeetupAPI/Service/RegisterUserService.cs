using FluentValidation;
using LanguageExt.Common;
using MeetupAPI.Entities;
using MeetupAPI.Models;
using MeetupAPI.Validators;
using Microsoft.AspNetCore.Identity;

namespace MeetupAPI.Service
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly RegisterUserValidator _validator;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly MeetupContext _meetupContext;

        public RegisterUserService(RegisterUserValidator validator,
            IPasswordHasher<User> passwordHasher,
            MeetupContext meetupContext)
        {
            _validator = validator;
            _passwordHasher = passwordHasher;
            _meetupContext = meetupContext;
        }

        public Result<User> CreateAsync(RegisterUserDto registerUserDto)
        {
            var validatorResult = _validator.Validate(registerUserDto);

            if (!validatorResult.IsValid)
            {
                var validationException = new ValidationException(validatorResult.Errors);
                return new Result<User>(validationException);
            }

            var newUser = new User
            {
                Email = registerUserDto.Email,
                Nationality = registerUserDto.Nationality,
                DateOfBirth = registerUserDto.DateOfBirth,
                PasswordHash = null,
                RoleId = registerUserDto.RoleId
            };

            var passwordHash = _passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = passwordHash;

            _meetupContext.Users.Add(newUser);
            _meetupContext.SaveChanges();

            return new Result<User>(newUser);
        }
    }
}
