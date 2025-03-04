# Habits Backend

## Migrations

### Create new migration

`dotnet ef migrations add <MigrationName> -p ./Habits.Infrastructure -s ./Habits.WebApi`

### Apply migrations

`dotnet ef database update <MigrationName> -p ./Habits.Infrastructure -s ./Habits.WebApi`