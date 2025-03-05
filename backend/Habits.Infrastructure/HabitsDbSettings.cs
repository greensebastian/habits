namespace Habits.Infrastructure;

public class HabitsDbSettings
{
    public string ConnectionString { get; set; } = "";

    public bool MigrateOnStartup { get; set; } = false;
}