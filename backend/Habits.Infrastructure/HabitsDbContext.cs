using Habits.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Habits.Infrastructure;

public class HabitsDbContext(DbContextOptions<HabitsDbContext> options, IOptions<HabitsDbSettings> settings) : DbContext(options)
{
    public DbSet<Habit> Habits { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var habitsModel = modelBuilder.Entity<Habit>();
        
        habitsModel.HasKey(habit => habit.Id);
        habitsModel.HasIndex(habit => habit.CreatedAt);
        habitsModel.OwnsMany(habit => habit.Entries, logEntryModel =>
        {
            logEntryModel.ToTable("LogEntries");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(settings.Value.ConnectionString);
}