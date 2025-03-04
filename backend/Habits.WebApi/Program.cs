using Habits.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.AddHabits();

var app = builder.Build();
app.UseHabits();

app.Run();
