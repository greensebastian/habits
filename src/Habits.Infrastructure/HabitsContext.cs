using Habits.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Habits.Infrastructure;

public class HabitsContext : DbContext
{
    private string ConnectionString { get; }
    
    public HabitsContext(IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connString))
            throw new ArgumentException("Configuration must contain default connection string", nameof(configuration));
        ConnectionString = connString;
    }
    
    public DbSet<UserProfile> UserProfiles { get; init; }
    public DbSet<Direction> Directions { get; init; }
    public DbSet<Habit> Habits { get; init; }
    public DbSet<LogEntry> LogEntries { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(ConnectionString, builder => builder.MigrationsAssembly("Habits.Infrastructure.Migrations"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>().HasKey(e => e.Id);
        modelBuilder.Entity<UserProfile>()
            .HasMany<Direction>()
            .WithOne()
            .HasForeignKey(e => e.UserProfileId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Direction>().HasKey(e => e.Id);
        modelBuilder.Entity<Direction>()
            .HasMany<Habit>()
            .WithOne()
            .HasForeignKey(e => e.DirectionId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Direction>().HasIndex(e => new { e.UserProfileId, e.Start, e.End });
        
        modelBuilder.Entity<Habit>().HasKey(e => e.Id);
        modelBuilder.Entity<Habit>()
            .HasMany<LogEntry>()
            .WithOne()
            .HasForeignKey(e => e.HabitId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Habit>().HasIndex(e => new { e.DirectionId, e.Start, e.End });
        
        modelBuilder.Entity<LogEntry>().HasKey(e => e.Id);
        modelBuilder.Entity<LogEntry>().HasIndex(e => new { e.HabitId, e.PerformedAt});

        var dateTimeConverter = new DateTimeOffsetValueConverter();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.PropertyInfo?.PropertyType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        }
    }
}