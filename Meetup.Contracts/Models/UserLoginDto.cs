namespace MeetupAPI.Models;

public class UserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}