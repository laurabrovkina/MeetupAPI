using Ardalis.ApiEndpoints;
using MeetupAPI.ApiEndpoints;
using MeetupAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class AccountRegister : EndpointBaseSync
    .WithRequest<AccountRegisterRequest>
    .WithActionResult<AccountRegisterResult>
{
    private readonly MeetupContext _meetupContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountRegister(MeetupContext meetupContext,
        IPasswordHasher<User> passwordHasher)
    {
        _meetupContext = meetupContext;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("/register")]

    public override ActionResult<AccountRegisterResult> Handle(AccountRegisterRequest request)
    {
        var newUser = new User
        {
            Email = request.Email,
            Nationality = request.Nationality,
            DateOfBirth = request.DateOfBirth,
            PasswordHash = null,
            RoleId = request.RoleId
        };

        var passwordHash = _passwordHasher.HashPassword(newUser, request.Password);
        newUser.PasswordHash = passwordHash;

        _meetupContext.Users.Add(newUser);
        _meetupContext.SaveChanges();

        var user = _meetupContext.Users
            .FirstOrDefault(x => x.Email == request.Email);

        return Ok(new AccountRegisterResult { Id = user.Id});
    }
}