namespace MeetupAPI.Models;

public class RegisterUserRequest : UserLoginRequest
{
    public string Nationality { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int RoleId { get; set; } = 1;
}