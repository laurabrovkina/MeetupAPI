using System;

namespace MeetupAPI.ApiEndpoints;

public class AccountRegisterRequest
{
    public string Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int RoleId { get; set; } = 1;
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}