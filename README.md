# habits

## Status

- [x] Domain
- [x] Database context
- [x] Migrations
- [x] CRUD
- [x] TestContainers
- [x] Postgres docker
- [x] gRPC (& client)
- [x] AWS Aurora
- [/] CDK Basics
- [/] App Docker
- [/] GitHub deploy
- [ ] Deletion
- [ ] AppRunner

## Usage

### Test
```bash
dotnet test --project src/Habits.IntegrationTest
```

### Setup infra locally
```bash
docker compose up
```

### Run migrations
```bash
dotnet ef database update --project src/Habits.Infrastructure.Migrations
```

### Run solution
```bash
dotnet run --project src/Habits.gRPC/
```