namespace MeetupAPI.MinimalApi.Requests;

public class CreateUserRequest
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string ConfirmPassword { get; init; } = default!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Nationality { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public int RoleId { get; init; } = 1;
}

