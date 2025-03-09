using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using ErrorHandling.Exceptions;
using Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Features.Account.Login;

public record LoginCommand(string Email, string Password) : IRequest<string>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly MeetupContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        MeetupContext context,
        IPasswordHasher<User> passwordHasher,
        IJwtProvider jwtProvider)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);

        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
        {
            throw new ApiResponseException(HttpStatusCode.BadRequest, "Invalid username or password");
        }

        return _jwtProvider.GenerateJwtToken(user);
    }
}