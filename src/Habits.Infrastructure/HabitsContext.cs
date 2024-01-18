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
    
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Direction> Directions { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(ConnectionString, builder => builder.MigrationsAssembly("Habits.Infrastructure.Migrations"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>().HasKey(e => e.Id);
        modelBuilder.Entity<Direction>().HasKey(e => e.Id);
        modelBuilder.Entity<Habit>().HasKey(e => e.Id);
        modelBuilder.Entity<LogEntry>().HasKey(e => e.Id);
    }
}