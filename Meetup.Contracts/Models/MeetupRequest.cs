namespace MeetupAPI.Models;

public class MeetupRequest
{
    public string Name { get; set; }
    public string Organizer { get; set; }
    public DateTime Date { get; set; }
    public bool IsPrivate { get; set; }
}
