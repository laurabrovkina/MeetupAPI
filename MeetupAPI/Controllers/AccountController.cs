using MeetupAPI.Authorization;
using MeetupAPI.Entities;
using MeetupAPI.Identity;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MeetupAPI.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly MeetupContext _meetupContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IAuthorizationService _authorizationService;

        public AccountController(MeetupContext meetupContext,
            IPasswordHasher<User> passwordHasher,
            IJwtProvider jwtProvider,
            IAuthorizationService authorizationService)
        {
            _meetupContext = meetupContext;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _authorizationService = authorizationService;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]RegisterUserDto registerUserDto)
        {
            var user = _meetupContext.Users
                .Include(user => user.Role)
                .FirstOrDefault(user => user.Email == registerUserDto.Email);

            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, registerUserDto.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid username or password");
            }

            var token = _jwtProvider.GenerateJwtToken(user);

            return Ok(token);
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody]RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            return Ok();
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Edit([FromBody] UpdateUserDto updateUserDto)
        {
            var user = _meetupContext.Users
                .FirstOrDefault(x => x.Email == updateUserDto.Email);

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Nationality = updateUserDto.Nationality;
            user.DateOfBirth = updateUserDto.DateOfBirth;
            user.RoleId= updateUserDto.RoleId;

            if (updateUserDto.Password is not null
                && updateUserDto.Password == updateUserDto.ConfirmPassword)
            {
                var passwordHash = _passwordHasher.HashPassword(user, updateUserDto.Password);
                user.PasswordHash = passwordHash;
            }

            _meetupContext.SaveChanges();

            return NoContent();
        }
    }
}
