namespace MeetupAPI.MinimalApi.Requests;

public class UserLoginDto
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string ConfirmPassword { get; init; } = default!;
}
