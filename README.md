<p align="center">
  <img width="500" height="300" alt="remitee_banner" src="https://github.com/user-attachments/assets/10f4a173-47f6-4742-97b7-3d45e6a360cb" />
</p>
<p align="center">
  <img width="1902" height="799" alt="Screenshot 2026-06-08 082742" src="https://github.com/user-attachments/assets/02a8c1b9-a93e-4a95-b981-357a5cb1bda7" />
</p>

# SweetMedical

Backend for the **Sweet Medical** technical challenge (Remitee): a REST API to manage medical appointments.

Built with **.NET 10** following **DDD**.

## Architecture

The solution is organized in layers, with dependencies always pointing toward the domain:

```
SweetMedical.Api            ──►  Application, Infrastructure, Contracts (presentation / composition)
SweetMedical.Infrastructure ──►  Application                            (persistence, external services)
SweetMedical.Application    ──►  Domain                                 (use cases, orchestration)
SweetMedical.Contracts      ──►  (no dependencies)                      (public API DTOs)
SweetMedical.Domain         ──►  (no dependencies)                      (entities, business rules)
```

| Project | Responsibility |
|---------|----------------|
| `src/SweetMedical.Domain` | Entities, value objects, business rules and invariants. No external dependencies. |
| `src/SweetMedical.Application` | Use cases, abstractions (ports), validation and orchestration. |
| `src/SweetMedical.Contracts` | Public DTOs (request/response) exposed by the API over HTTP. No dependencies. |
| `src/SweetMedical.Infrastructure` | Persistence and external-service implementations (adapters). |
| `src/SweetMedical.Api` | REST API (controllers, error handling), configuration and dependency composition. |
| `tests/SweetMedical.Tests` | Unit tests. |

## Requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)

## Running the backend

### Option A — Docker Compose (recommended)

Spins up the API together with a SQL Server instance. From the repository root:

```bash
docker compose up --build
```

- API: `http://localhost:8080`
- SQL Server: `localhost,1433` (user `sa`, database `SweetMedical`)

The API container applies EF Core migrations and seeds the doctors automatically on
startup, so the database is ready to use. To stop and remove everything (including the
database volume):

```bash
docker compose down -v
```

### Option B — Local (`dotnet run`)

Requires a reachable SQL Server matching the connection string in
`src/SweetMedical.Api/appsettings.json` (defaults to `localhost,1433`). The easiest way is to
start only the database with Compose and run the API locally:

```bash
docker compose up -d db
dotnet run --project src/SweetMedical.Api
```

By default the API is available at the URLs defined in
`src/SweetMedical.Api/Properties/launchSettings.json`.

In the `Development` environment the following are available:

- **Swagger UI**: `/api/docs`
- **OpenAPI document (JSON)**: `/swagger/v1/swagger.json`

Both are provided by `Swashbuckle.AspNetCore`.

## Tests

```bash
# Run all tests
dotnet test tests/SweetMedical.Tests/SweetMedical.Tests.csproj

# Run a single test class
dotnet test tests/SweetMedical.Tests/SweetMedical.Tests.csproj --filter "FullyQualifiedName~CreateAppointmentCommandHandlerTests"
```

## Technical decisions

- **Clean Architecture / DDD**: layered separation to isolate business rules from infrastructure
  details, improving testability and extensibility.
- **`.slnx` solution format**: the new XML-based solution format supported by .NET 10.
- **Swagger UI + OpenAPI**: documentación y esquema JSON expuestos via `Swashbuckle.AspNetCore` en el entorno `Development`.
- **ErrorOr**: handlers return `ErrorOr<T>` instead of throwing. Controllers map errors to a `DomainException` which is caught by `GlobalExceptionHandler : IExceptionHandler`, translating it to the appropriate `ProblemDetails` response.
- **MediatR + FluentValidation pipeline**: a `ValidateBehavior<TRequest, TResponse>` runs automatically before every handler that has a registered validator.
- **Moq**: unit tests mock repositories via `Mock<IRepository>` instead of hand-written fakes.

## AI usage

**Used AI for:**

- Scaffolding the Clean Architecture (DDD) solution in .NET 10: `Domain`, `Application`,
  `Contracts`, `Infrastructure`, `Api` and `Tests` projects, plus their project references.
- Base DDD building blocks (`Entity<TId>`, `AggregateRoot<TId>`) and the per-layer
  *composition root* extension methods (`AddApplication`, `AddInfrastructure`).
- Wiring OpenAPI + Swagger UI and writing this README.
- `GlobalExceptionHandler`, `DomainException`, and the `ValidateBehavior` pipeline.
- Unit test structure and Moq-based tests.

**Built manually:**

- Domain modeling: `Appointment` and `Doctor` entities with their business rules and invariants.
- All use cases: commands, queries, handlers and result types (`CreateAppointment`, `CancelAppointment`, `GetAppointments`, `GetDoctors`).
- Repository implementations (`DoctorRepository`, `AppointmentRepository`) and EF Core configurations.
- Controllers (`DoctorController`, `AppointmentController`) with request mapping and response handling.
- Contracts: all request/response DTOs and AutoMapper profiles.
- Docker Compose setup with SQL Server and API container.
- Error handling strategy: `ErrorOr` → `DomainException` → `GlobalExceptionHandler`.

## Next steps / Future improvements

- **Global appointments endpoint**: add `GET /appointments` without requiring `doctorId`, enabling server-side pagination across all doctors. Currently the frontend fetches per doctor and paginates client-side.
- **Appointment rescheduling**: add a `PATCH /appointments/{id}/reschedule` endpoint to change the date/time of an active appointment instead of only cancelling it.
- **Authentication**: protect endpoints with JWT Bearer tokens.
- Add filtering by status on `GET /appointments` (e.g. only `Active`).
- Add UnitOfWork
