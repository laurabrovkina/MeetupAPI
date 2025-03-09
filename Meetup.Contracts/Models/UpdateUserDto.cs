namespace Meetup.Contracts.Models;

public class UpdateUserDto : UserLoginDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Nationality { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int RoleId { get; set; } = 1;
}