# SweetMedical

Backend for the **Sweet Medical** technical challenge (Remitee): a REST API to manage medical appointments.

Built with **.NET 10** following **Clean Architecture (DDD)**.

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
| `src/SweetMedical.Api` | REST API (controllers), configuration and dependency composition. |
| `tests/SweetMedical.UnitTests` | Unit tests. |

### Contracts vs. Application models

`Contracts` defines the **stable HTTP contract** consumed by the client (React / React Native),
decoupled from both the domain entities and the internal commands/queries of `Application`.

- The domain is **never** exposed directly through the API.
- `Application` does **not** reference `Contracts`: it has its own use-case models.
- The `Api` maps `request (Contracts) → command/query (Application)` and
  `result (Application) → response (Contracts)`.

Each layer exposes an extension method (`AddApplication`, `AddInfrastructure`) that acts as a
*composition root* and is invoked from `Program.cs`.

## Requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)

## Running the backend

From the repository root:

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run the API
dotnet run --project src/SweetMedical.Api
```

By default the API is available at the URLs defined in
`src/SweetMedical.Api/Properties/launchSettings.json`.

In the `Development` environment the following are available:

- **Swagger UI**: `/swagger`
- **OpenAPI document**: `/openapi/v1.json`

The OpenAPI document is generated with the native .NET support (`Microsoft.AspNetCore.OpenApi`)
and the UI is rendered with `Swashbuckle.AspNetCore.SwaggerUI` pointing to that document.

## Tests

```bash
dotnet test
```

## Technical decisions

- **Clean Architecture / DDD**: layered separation to isolate business rules from infrastructure
  details, improving testability and extensibility.
- **`.slnx` solution format**: the new XML-based solution format supported by .NET 10.
- **OpenAPI** integrated via `Microsoft.AspNetCore.OpenApi`, with **Swagger UI**
  (`Swashbuckle.AspNetCore.SwaggerUI`) exposed only in the `Development` environment.

## AI usage

**Used AI for:**

- Scaffolding the Clean Architecture (DDD) solution in .NET 10: `Domain`, `Application`,
  `Contracts`, `Infrastructure`, `Api` and `UnitTests` projects, plus their project references.
- Base DDD building blocks (`Entity<TId>`, `AggregateRoot<TId>`) and the per-layer
  *composition root* extension methods (`AddApplication`, `AddInfrastructure`).
- Wiring OpenAPI + Swagger UI and writing this README.

**Manually reviewed / corrected:**

- Simplified `Program.cs` (service/pipeline grouping, removed the sample endpoints).
- Reviewed and adjusted the README architecture section.
- Domain modeling and business rules are written manually.

## Next steps / Future improvements

- Model the appointment domain (entities such as `Doctor`, `Appointment`, etc.) in `SweetMedical.Domain`.
- Implement the use cases in `SweetMedical.Application`.
- Define the request/response DTOs in `SweetMedical.Contracts` (e.g. by feature: `Appointments/`, `Doctors/`).
- Define persistence in `SweetMedical.Infrastructure` (EF Core or another provider).
- Expose the REST endpoints (list doctors, list appointments, create appointment, cancel appointment).
- Increase test coverage.
