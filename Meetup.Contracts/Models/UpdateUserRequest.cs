namespace MeetupAPI.Models;

public class UpdateUserRequest : UserLoginRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Nationality { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int RoleId { get; set; } = 1;
}