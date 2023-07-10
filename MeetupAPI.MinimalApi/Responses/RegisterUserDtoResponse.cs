namespace MinimalApi.Responses;

public class RegisterUserDtoResponse
{
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string Nationality { get; init; } = default!;

    public DateTime? DateOfBirth { get; init; }

    public int RoleId { get; init; }

    public string Email { get; init; } = default!;
}
