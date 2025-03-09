using System;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Features.Account.Register
{
    public record RegisterCommand(
        string Email,
        string Password,
        string Nationality,
        DateTime DateOfBirth,
        int RoleId) : IRequest<string>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly MeetupContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterCommandHandler(
            MeetupContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                Email = request.Email,
                Nationality = request.Nationality,
                DateOfBirth = request.DateOfBirth,
                RoleId = request.RoleId,
                PasswordHash = _passwordHasher.HashPassword(null, request.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return newUser.Email;
        }
    }
}