﻿using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Entities;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meetup>()
            .HasOne(u => u.CreatedBy);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role);

        modelBuilder.Entity<Meetup>()
            .HasOne(m => m.Location)
            .WithOne(l => l.Meetup)
            .HasForeignKey<Location>(l => l.MeetupId);

        modelBuilder.Entity<Meetup>()
            .HasMany(m => m.Lectures)
            .WithOne(l => l.Meetup);
    }
}