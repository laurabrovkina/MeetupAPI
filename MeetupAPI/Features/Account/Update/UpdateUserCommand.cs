using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using ErrorHandling.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Features.Account.Update;

public record UpdateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Nationality,
    DateTime? DateOfBirth,
    int RoleId) : IRequest;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly MeetupContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UpdateUserCommandHandler(
        MeetupContext context,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, "User not found");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Nationality = request.Nationality;
        user.DateOfBirth = request.DateOfBirth;
        user.RoleId = request.RoleId;

        if (!string.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}