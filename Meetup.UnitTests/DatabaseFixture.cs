using System;
using MeetupAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetup.UnitTests;

public class DatabaseFixture : IDisposable
{
    public readonly MeetupContext MeetupContext;

    public DatabaseFixture()
    {
        MeetupContext = new MeetupContext((DbContextOptions<MeetupContext>?)
            new DbContextOptionsBuilder<MeetupContext>()
                .UseInMemoryDatabase("MeetupDb")
                .Options);

        MeetupContext.Roles.Add(new Role { Id = 1, RoleName = "Moderator" });
        MeetupContext.Roles.Add(new Role { Id = 2, RoleName = "User" });
        MeetupContext.Roles.Add(new Role { Id = 3, RoleName = "Admin" });
        MeetupContext.Users.Add(new User { Email = "existing@example.com" });
        MeetupContext.SaveChanges();
    }

    public void Dispose()
    {
        MeetupContext.Dispose();
    }
}