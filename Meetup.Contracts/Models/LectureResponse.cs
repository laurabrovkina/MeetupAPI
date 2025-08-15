namespace MeetupAPI.Models;

public class LectureResponse
{
    public List<Lecture> Lectures { get; set; }
    public string MeetupName { get; set; }
    public string MeetupOrganizer { get; set; }
}

public class Lecture
{
    public string Author { get; set; }
    public string Topic { get; set; }
    public string? Description { get; set; }
}