using MinimalApi.Models;
using MinimalApi.Responses;

namespace MinimalApi.Mappings;

public static class RegisterUserDtoExtension
{
    public static RegisterUserDtoResponse ToRegisterUserDtoResponse(this User request)
    {
        return new RegisterUserDtoResponse
        {
            FirstName = request?.FirstName,
            LastName = request?.LastName,
            Email = request?.Email,
            Nationality = request?.Nationality,
            DateOfBirth = request?.DateOfBirth,
            RoleId = request.RoleId
        };
    }
}