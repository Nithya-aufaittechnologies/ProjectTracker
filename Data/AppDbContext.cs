using Microsoft.EntityFrameworkCore;
using MpcFabricsTracker.Models;

namespace MpcFabricsTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Developer> Developers => Set<Developer>();
    public DbSet<Week> Weeks => Set<Week>();
    public DbSet<TrackerEntry> TrackerEntries => Set<TrackerEntry>();
    public DbSet<WeeklyCapacity> WeeklyCapacities => Set<WeeklyCapacity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Developer>()
            .HasIndex(d => d.Name)
            .IsUnique();

        modelBuilder.Entity<Week>()
            .HasIndex(w => w.WeekNumber)
            .IsUnique();

        modelBuilder.Entity<WeeklyCapacity>()
            .HasIndex(c => new { c.WeekId, c.DeveloperId })
            .IsUnique();

        modelBuilder.Entity<TrackerEntry>()
            .HasOne(t => t.Week)
            .WithMany(w => w.TrackerEntries)
            .HasForeignKey(t => t.WeekId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TrackerEntry>()
            .HasOne(t => t.Developer)
            .WithMany(d => d.TrackerEntries)
            .HasForeignKey(t => t.DeveloperId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WeeklyCapacity>()
            .HasOne(c => c.Week)
            .WithMany(w => w.WeeklyCapacities)
            .HasForeignKey(c => c.WeekId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WeeklyCapacity>()
            .HasOne(c => c.Developer)
            .WithMany(d => d.WeeklyCapacities)
            .HasForeignKey(c => c.DeveloperId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
