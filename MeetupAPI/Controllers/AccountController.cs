using MeetupAPI.Entities;
using MeetupAPI.Identity;
using MeetupAPI.Models;
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

        public AccountController(MeetupContext meetupContext, IPasswordHasher<User> passwordHasher, IJwtProvider jwtProvider)
        {
            _meetupContext = meetupContext;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
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
    }
}
