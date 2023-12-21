using System;
using MeetupAPI.Entities;
using Microsoft.EntityFrameworkCore;

public class DatabaseFixture : IDisposable
{
    public readonly MeetupContext _meetupContext;

    public DatabaseFixture()
    {
        _meetupContext = new MeetupContext((DbContextOptions<MeetupContext>?)
        new DbContextOptionsBuilder<MeetupContext>()
                .UseInMemoryDatabase(databaseName: "MeetupDb")
                .Options);

        _meetupContext.Roles.Add(new Role { Id = 1, RoleName = "Moderator" });
        _meetupContext.Roles.Add(new Role { Id = 2, RoleName = "User" });
        _meetupContext.Roles.Add(new Role { Id = 3, RoleName = "Admin" });
        _meetupContext.Users.Add(new User { Email = "existing@example.com" });
        _meetupContext.SaveChanges();
    }

    public void Dispose()
    {
        _meetupContext.Dispose();
    }
}