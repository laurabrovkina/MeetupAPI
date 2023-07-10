namespace MeetupAPI.MinimalApi.Requests;

public class RegisterUserDtoRequest : UserLoginDto
{
    public string Nationality { get; init; } = default!;

    public DateTime? DateOfBirth { get; init; }

    public int RoleId { get; init; } = 1;
}

