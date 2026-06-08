# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run the API locally (requires SQL Server — see Docker section)
dotnet run --project src/SweetMedical.Api

# Run all tests
dotnet test tests/SweetMedical.Tests/SweetMedical.Tests.csproj

# Run a single test by name
dotnet test tests/SweetMedical.Tests/SweetMedical.Tests.csproj --filter "FullyQualifiedName~CreateAppointmentCommandHandlerTests"

# Add a new EF migration
dotnet ef migrations add <MigrationName> --project src/SweetMedical.Infrastructure --startup-project src/SweetMedical.Api
```

## Docker

All compose files live under `compose/`. Copy `.env.example` to `.env` and adjust if needed before starting.

```bash
# Start API + SQL Server
docker compose -f compose/docker-compose.yml up --build

# Start only the database (then run API locally)
docker compose -f compose/docker-compose.yml up -d db

# Tear down including the database volume
docker compose -f compose/docker-compose.yml down -v
```

- API: `http://localhost:8080`
- SQL Server: `localhost,1433` (`sa` / password from `.env`)
- Swagger UI (Development only): `/api/docs`

The API applies EF Core migrations and seeds data automatically on startup via `DbSeeder.SeedAsync`.

## Architecture

Clean Architecture. Dependency rule: outer layers depend on inner layers, never the reverse.

```
SweetMedical.Domain          ← entities, aggregate roots, business rules — no dependencies
SweetMedical.Application     ← use cases, repository interfaces — depends on Domain
SweetMedical.Infrastructure  ← EF Core, SQL Server — depends on Application
SweetMedical.Api             ← controllers, AutoMapper, error handling — depends on Application + Infrastructure
SweetMedical.Contracts       ← request/response DTOs — no dependencies
```

### Request flow

```
HTTP Request
  → Controller ([ApiController] + ControllerBase)
  → MediatR Command/Query
  → ValidateBehavior (FluentValidation pipeline — runs automatically if a validator exists)
  → CommandHandler / QueryHandler → ErrorOr<T>
  → Controller maps result: success → Ok/Created, error → throw DomainException
  → GlobalExceptionHandler → ProblemDetails response
```

### Error handling

- Handlers return `ErrorOr<T>` — never throw.
- Controllers call `.Match(success => ..., errors => throw new DomainException(errors))`.
- `GlobalExceptionHandler : IExceptionHandler` (in `Api/Errors/`) maps `ErrorType` to HTTP status:
  - `Validation` → 400, `NotFound` → 404, `Conflict` → 409, `Unauthorized` → 403, everything else → 500.

### Application layer conventions

- Each use case in its own folder: `Appointments/Commands/CreateAppointment/` — command, handler, and optional validator.
- Result types in sibling `Common/` folders: `Appointments/Common/GetAppointments/GetAppointmentResult.cs`.
- Repository interfaces in `Application/Common/Interfaces/Persistence/`.

### AutoMapper

Profiles live in `Api/Mappings/`. When mapping to a positional record (primary-constructor only), use `ConstructUsing` — AutoMapper cannot instantiate positional records with property-setter mapping:

```csharp
CreateMap<GetAppointmentResult, GetAppointmentsResponse>()
    .ConstructUsing((src, ctx) => new GetAppointmentsResponse(
        ctx.Mapper.Map<List<AppointmentResponse>>(src.Appointments),
        src.TotalCount, src.Page, src.PageSize,
        (int)Math.Ceiling(src.TotalCount / (double)src.PageSize)));
```

### Infrastructure

- SQL Server via EF Core. Connection string key: `"Default"`.
- `DbSeeder.SeedAsync` runs at startup: applies pending migrations, then seeds doctors and appointments if tables are empty.

### Tests

The test project references `SweetMedical.Api` (the full stack). It uses:
- **xunit.v3** for test discovery and assertions.
- **Microsoft.EntityFrameworkCore.InMemory** for repository tests without a real database.
- **Microsoft.AspNetCore.Mvc.Testing** for potential integration-style tests.

Test folder structure mirrors source:
- Domain tests: `tests/SweetMedical.Tests/SweetMedical.Domain/Appointment/`
- Handler tests: `tests/SweetMedical.Tests/SweetMedical.Application/Appointments/Commands/<CommandName>/` or `Doctors/Queries/<QueryName>/`
