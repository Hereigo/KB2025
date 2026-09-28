# Calendarium — agent instructions

ASP.NET Core MVC calendar app on .NET 9, organized as a Clean Architecture solution (`Calendarium.sln`). Much of it is still skeleton code — empty classes and folders are placeholders to be filled, not dead code to delete.

## Layers and the dependency rule

| Project | References | Contains |
|---------|-----------|----------|
| `Domain` | nothing | Entities (`CalEvent`, `RequestHeader`), enums, `Constants` |
| `Application` | `Domain` | `Abstractions/` (interfaces), `Services/`, `CmdQryHandlers/`, `Mappers/`, `Validators/`, `Specifications/`, `Exceptions/` |
| `Infrastructure` | `Application` | `Persistanse/` (EF Core `CalDbContext`, repository, caching, seeding), `Notifications/`, `Identity/`, `ExternalServices/`, `Migrations/` |
| `WebApp` | `Application`, `Infrastructure` | Controllers, Razor views, Identity pages, `Program.cs` composition root |

Dependencies point inward. Do not add a `ProjectReference` that contradicts this table.

- `Domain` and `Application` must not reference EF Core, ASP.NET Core, AutoMapper, or any DB/HTTP/SMTP client.
- `Application` depends on interfaces in `Application/Abstractions/`; `Infrastructure` implements them.
- `Program.cs` is the only place concrete Infrastructure types are wired to interfaces.
- Controllers call Application services/handlers — never `CalDbContext` or `CalendRepository` directly.

## Conventions

- Target framework `net9.0`, nullable enabled, implicit usings enabled — omit redundant `using System;` blocks in new files.
- `Persistanse` is the existing (misspelled) folder and namespace for persistence. Match it; do not silently rename.
- `Application/Abstractions/` currently declares the `Application.Interfaces` namespace. Keep new abstractions consistent with the files around them rather than introducing a third variant.
- `WebApp/Models/` holds view models. They mirror Domain types by design — do not consolidate them into Domain.
- EF Core is SQL Server; connection string `CalDbContextConnection` in `appsettings.json`.

## Commands

```pwsh
dotnet build Calendarium.sln
dotnet run --project WebApp
dotnet ef migrations add <Name> --project Infrastructure --startup-project WebApp
dotnet ef database update --project Infrastructure --startup-project WebApp
```

## Boundaries

- Do not edit `Infrastructure/Migrations/` by hand; generate migrations with the CLI.
- Do not touch `bin/`, `obj/`, or `wwwroot/lib/`.
- `appsettings.json` holds connection strings — never commit real credentials or print them in output.
