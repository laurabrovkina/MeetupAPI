namespace Meetup.Contracts.Models;

public class RegisterUserDto : UserLoginDto
{
    public string Nationality { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int RoleId { get; set; } = 1;
}