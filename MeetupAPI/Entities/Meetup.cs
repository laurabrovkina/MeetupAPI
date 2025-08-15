using System;
using System.Collections.Generic;
using MeetupAPI.Models;

namespace Entities;

public class Meetup : AggregateRoot
{
    // we should set a private constructor to make this object immutable
    // private Meetup() { } EF Core still needs it
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Organizer { get; set; }
    public DateTime Date { get; set; }
    public bool IsPrivate { get; set; }
    public virtual Location Location { get; set; }
    public virtual List<Lecture> Lectures { get; set; }
    public int? CreatedById { get; set; }
    public User CreatedBy { get; set; }

    // static factory method that you use instead of constructor to give you back your entity
    public static Meetup Create(MeetupRequest model, string userId)
    {
        var meetup = new Meetup
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Organizer = model.Organizer,
            Date = model.Date,
            IsPrivate = model.IsPrivate,
            CreatedById = int.Parse(userId)
        };
        
        meetup.AddDomainEvent(new MeetupCreatedDomainEvent(meetup.Id, userId));
        return meetup;
    }
}