using Microsoft.EntityFrameworkCore;

namespace Entities;

public class MeetupContext : DbContext
{
    public MeetupContext(DbContextOptions<MeetupContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Meetup> Meetups { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meetup>()
            .HasOne(u => u.CreatedBy);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Meetup>()
            .HasOne(m => m.Location)
            .WithOne(l => l.Meetup)
            .HasForeignKey<Location>(l => l.MeetupId);

        modelBuilder.Entity<Meetup>()
            .HasMany(m => m.Lectures)
            .WithOne(l => l.Meetup);

        modelBuilder.Entity<RefreshToken>()
            .HasKey(r => r.Id);
        
        modelBuilder.Entity<RefreshToken>()
            .Property(r => r.Token).HasMaxLength(200);
        
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(r => r.Token).IsUnique();
            
        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}