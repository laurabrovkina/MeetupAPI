using MeetupAPI.MinimalApi.Requests;
using MinimalApi.Models;

namespace MinimalApi.Mappings;

public static class UserExtension
{
    public static User ToUser(this RegisterUserDtoRequest request)
    {
        return new User
        {
            Email = request.Email,
            Nationality = request.Nationality,
            DateOfBirth = request.DateOfBirth,
            PasswordHash = null,
            RoleId = request.RoleId
        };
    }
}