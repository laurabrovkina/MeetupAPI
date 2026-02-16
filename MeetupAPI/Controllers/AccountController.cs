using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Entities;
using ErrorHandling;
using ErrorHandling.Exceptions;
using Identity;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(MeetupContext meetupContext,
        IPasswordHasher<User> passwordHasher,
        IJwtProvider jwtProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        _meetupContext = meetupContext;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _httpContextAccessor = httpContextAccessor;
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
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _jwtProvider.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };
        
        var response = new RegisterUserResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token
        };
        
        _meetupContext.RefreshTokens.Add(refreshToken);
         _meetupContext.SaveChanges();

        return Ok(response);
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

    [HttpPost("refresh-token")]
    public ActionResult LoginWithRefreshToken([FromBody]RefreshTokenRequest refreshTokenRequest)
    {
        var refreshToken = _meetupContext.RefreshTokens
            .Include(r => r.User)
                .ThenInclude(u => u.Role)
            .FirstOrDefault(r => r.Token == refreshTokenRequest.RefreshToken);

        if (refreshToken == null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            throw new ApiResponseException(HttpStatusCode.BadRequest,"The refresh token has expired");
        }
        
        var accessToken = _jwtProvider.GenerateJwtToken(refreshToken.User);
        
        refreshToken.Token = _jwtProvider.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);
        
        _meetupContext.SaveChanges();

        var result = new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };

        return Ok(result);
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

    [HttpDelete("{userId}/refresh-tokens")]
    public ActionResult RevokeRefreshTokens([FromRoute]int userId)
    {
        if (userId != GetCurrentUserId())
        {
            throw new ApiResponseException(HttpStatusCode.Forbidden, "You are not authorized to revoke");
        }
        
        _meetupContext.RefreshTokens
            .Where(r => r.UserId == userId)
            .ExecuteDelete();
        
        return NoContent();
    }

    private int? GetCurrentUserId()
    {
        int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            out var currentUserId);
        
        return currentUserId > 0
            ? currentUserId
            : null;
    }
}