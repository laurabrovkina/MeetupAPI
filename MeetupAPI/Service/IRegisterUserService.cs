using LanguageExt.Common;
using MeetupAPI.Entities;
using MeetupAPI.Models;

namespace MeetupAPI.Service
{
    public interface IRegisterUserService
    {
        Result<User> CreateAsync(RegisterUserDto registerUserDto);
    }
}
