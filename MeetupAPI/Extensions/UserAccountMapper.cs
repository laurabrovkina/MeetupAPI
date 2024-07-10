using MeetupAPI.Entities;
using MeetupAPI.Models;

namespace MeetupAPI.Extensions;

public static class UserAccountMapper
{
    public static User ToDomain(this RegisterUserDto registerUserDto)
    {
        return new User
        {
            Email = registerUserDto.Email,
            RoleId = registerUserDto.RoleId,
            Nationality = registerUserDto.Nationality,
            DateOfBirth = registerUserDto.DateOfBirth
        };
    }

    public static RegisterUserDto ToDto(this User user) 
    {
        return new RegisterUserDto
        {
            Email = user.Email,
            RoleId = user.RoleId,
            Nationality = user.Nationality,
            DateOfBirth = user.DateOfBirth
        };
    }
}
