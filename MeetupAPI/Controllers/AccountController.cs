using System.Linq;
using System.Net;
using Entities;
using ErrorHandling;
using ErrorHandling.Exceptions;
using Identity;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers;

[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IJwtProvider _jwtProvider;
    private readonly MeetupContext _meetupContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountController(MeetupContext meetupContext,
        IPasswordHasher<User> passwordHasher,
        IJwtProvider jwtProvider)
    {
        _meetupContext = meetupContext;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody]RegisterUserRequest registerUserRequest)
    {
        var user = _meetupContext.Users
            .Include(user => user.Role)
            .FirstOrDefault(user => user.Email == registerUserRequest.Email);

        if (user == null)
        {
            throw new ApiResponseException(HttpStatusCode.BadRequest, "Invalid username or password");
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, registerUserRequest.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new ApiResponseException(HttpStatusCode.BadRequest, "Invalid username or password");
        }

        var token = _jwtProvider.GenerateJwtToken(user);

        return Ok(token);
    }

    [HttpPost("register")]
    public ActionResult Register([FromBody]RegisterUserRequest registerUserRequest)
    {
        if (!ModelState.IsValid)
        {
            ErrorMessages.BadRequestMessage(registerUserRequest, ModelState);
        }

        var newUser = new User
        {
            Email = registerUserRequest.Email,
            Nationality = registerUserRequest.Nationality,
            DateOfBirth = registerUserRequest.DateOfBirth,
            PasswordHash = null,
            RoleId = registerUserRequest.RoleId
        };

        var passwordHash = _passwordHasher.HashPassword(newUser, registerUserRequest.Password);
        newUser.PasswordHash = passwordHash;

        _meetupContext.Users.Add(newUser);
        _meetupContext.SaveChanges();

        return Ok();
    }

    [HttpPut("edit")]
    [Authorize(Roles = "Admin,Moderator")]
    public ActionResult Edit([FromBody] UpdateUserRequest updateUserRequest)
    {
        var user = _meetupContext.Users
            .FirstOrDefault(x => x.Email == updateUserRequest.Email);

        if (user == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, $"User with email {updateUserRequest.Email} has not been found");
        }

        if (!ModelState.IsValid)
        {
            ErrorMessages.BadRequestMessage(updateUserRequest, ModelState);
        }

        user.FirstName = updateUserRequest.FirstName;
        user.LastName = updateUserRequest.LastName;
        user.Nationality = updateUserRequest.Nationality;
        user.DateOfBirth = updateUserRequest.DateOfBirth;
        user.RoleId= updateUserRequest.RoleId;

        if (updateUserRequest.Password == updateUserRequest.ConfirmPassword)
        {
            var passwordHash = _passwordHasher.HashPassword(user, updateUserRequest.Password);
            user.PasswordHash = passwordHash;
        }

        _meetupContext.SaveChanges();

        return NoContent();
    }
}