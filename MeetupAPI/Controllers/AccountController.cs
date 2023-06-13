using MeetupAPI.Entities;
using MeetupAPI.Identity;
using MeetupAPI.Models;
using MeetupAPI.Service;
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
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRegisterUserService _registerUserService;

        public AccountController(MeetupContext meetupContext,
            IJwtProvider jwtProvider,
            IRegisterUserService registerUserService,
            IPasswordHasher<User> passwordHasher)
        {
            _meetupContext = meetupContext;
            _jwtProvider = jwtProvider;
            _registerUserService = registerUserService;
            _passwordHasher = passwordHasher;
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
        public IActionResult Register([FromBody]RegisterUserDto registerUserDto)
        {
            var result = _registerUserService.CreateAsync(registerUserDto);

            return result.ToOk();
        }
    }
}
