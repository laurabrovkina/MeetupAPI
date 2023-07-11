namespace MinimalApi.Responses;

public class UserResponse
{
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? Nationality { get; init; }

    public DateTime? DateOfBirth { get; init; }

    public int RoleId { get; init; }

    public string Email { get; init; } = default!;
}
