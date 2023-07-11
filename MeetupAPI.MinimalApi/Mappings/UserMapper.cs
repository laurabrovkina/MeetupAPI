using MeetupAPI.MinimalApi.Requests;
using MinimalApi.Models;

namespace MinimalApi.Mappings;

public static class UserMapper
{
    public static User ToUser(this CreateUserRequest request)
    {
        return new User
        {
            FirstName = request?.FirstName,
            LastName = request?.LastName,
            Email = request.Email,
            Nationality = request?.Nationality,
            DateOfBirth = request?.DateOfBirth,
            PasswordHash = null,
            RoleId = request.RoleId
        };
    }
}