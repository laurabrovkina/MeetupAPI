using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;

namespace MeetupAPI.MinimalApi;

public class MeetupContext : DbContext
{

    public MeetupContext(DbContextOptions<MeetupContext> options)
        : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role);
    }
}

