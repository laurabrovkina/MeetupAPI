using MeetupAPI.MinimalApi.Requests;
using MinimalApi.Models;
using MinimalApi.Responses;

namespace MinimalApi.Mappings;

public static class UserResponseMapper
{
    public static UserResponse ToUserResponse(this User request)
    {
        return new UserResponse
        {
            FirstName = request?.FirstName,
            LastName = request?.LastName,
            Email = request.Email,
            Nationality = request?.Nationality,
            DateOfBirth = request?.DateOfBirth,
            RoleId = request.RoleId
        };
    }

    public static UserResponse ToUserResponse(this CreateUserRequest request)
    {
        return new UserResponse
        {
            FirstName = request?.FirstName,
            LastName = request?.LastName,
            Email = request.Email,
            Nationality = request?.Nationality,
            DateOfBirth = request?.DateOfBirth,
            RoleId = request.RoleId
        };
    }
}