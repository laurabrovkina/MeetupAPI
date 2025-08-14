using System;

namespace Entities;

public class Location
{
    public int Id { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostCode { get; set; }

    public virtual Meetup Meetup { get; set; }
    public Guid MeetupId { get; set; }
}