using MeetupAPI.MinimalApi.Requests;
using MinimalApi.Responses;

namespace MinimalApi.Mappings;

public static class RegisterUserDtoResponseExtension
{
    public static RegisterUserDtoResponse ToRegisterUserDtoResponse(this RegisterUserDtoRequest request)
    {
        return new RegisterUserDtoResponse
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Nationality = request.Nationality,
            DateOfBirth = request.DateOfBirth,
            RoleId = request.RoleId
        };
    }
}