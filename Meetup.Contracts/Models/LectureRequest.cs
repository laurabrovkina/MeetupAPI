using System.ComponentModel.DataAnnotations;

namespace MeetupAPI.Models;

public class LectureRequest
{
    [Required]
    [MinLength(5)]
    public string Author { get; set; }
    [Required]
    [MinLength(5)]
    public string Topic { get; set; }
    public string Description { get; set; }
}
