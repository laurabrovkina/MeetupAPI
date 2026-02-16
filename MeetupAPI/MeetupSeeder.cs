using System;
using System.Collections.Generic;
using System.Linq;
using Entities;

public class MeetupSeeder
{
    private readonly MeetupContext _meetupContext;

    public MeetupSeeder(MeetupContext meetupContext)
    {
        _meetupContext = meetupContext;
    }

    public void Seed()
    {
        if (_meetupContext.Database.CanConnect())
        {
            if (!_meetupContext.Roles.Any())
            {
                InsertBasicRoles();
            }
            
            if (!_meetupContext.Meetups.Any())
            {
                InsertSampleData();
            }
        }
    }

    // INSERT INTO 
    //     [MeetupDb].[dbo].[Roles]
    // VALUES ('User'),('Moderator'),('Admin')
    private void InsertBasicRoles()
    {
        var roles = new List<Role>
        {
            new()
            {
                RoleName = "User"
            },
            new()
            {
                RoleName = "Moderator"
            },
            new()
            {
                RoleName = "Admin"
            }
        };
        
        _meetupContext.Roles.AddRange(roles);
        _meetupContext.SaveChanges();
    }

    private void InsertSampleData()
    {
        var meetups = new List<Meetup>
        {
            new()
            {
                Name = "Web summit",
                Date = DateTime.Now.AddDays(7),
                IsPrivate = false,
                Organizer = "Microsoft",
                Location = new Location
                {
                    City = "Krakow",
                    Street = "Szeroka 33/5",
                    PostCode = "31-337"
                },
                Lectures = new List<Lecture>
                {
                    new()
                    {
                        Author = "Bob Clark",
                        Topic = "Modern browsers",
                        Description = "Deep dive into V8"
                    }
                }
            },
            new()
            {
                Name = "4Devs",
                Date = DateTime.Now.AddDays(7),
                IsPrivate = false,
                Organizer = "KGD",
                Location = new Location
                {
                    City = "Warszawa",
                    Street = "Chmielna 33/5",
                    PostCode = "00-007"
                },
                Lectures = new List<Lecture>
                {
                    new()
                    {
                        Author = "Will Smith",
                        Topic = "React.js",
                        Description = "Redux introduction"
                    },
                    new()
                    {
                        Author = "John Cena",
                        Topic = "Angular store",
                        Description = "Ngxs in practice"
                    }
                }
            },
        };

        _meetupContext.AddRange(meetups);
        _meetupContext.SaveChanges();
    }
}