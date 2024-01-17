// See https://aka.ms/new-console-template for more information

using Habits.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddDbContext<HabitsContext>();
});

var host = builder.Build();