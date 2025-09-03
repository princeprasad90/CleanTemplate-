# CleanTemplate

This repository provides a minimal .NET 8 microservice template following a clean architecture approach with simple event sourcing backed by Entity Framework Core.

## Projects

- **CleanTemplate.Domain** – Domain entities and domain events.
- **CleanTemplate.Application** – Application services and abstractions.
- **CleanTemplate.Infrastructure** – EF Core based event store implementation.
- **CleanTemplate.Api** – ASP.NET Core API exposing sample endpoints.
- **CleanTemplate.Tests** – xUnit tests demonstrating basic usage.

## Getting started

Ensure the .NET 8 SDK is installed.

```bash
dotnet restore
dotnet test
dotnet run --project src/CleanTemplate.Api
```

The API exposes sample endpoints to create orders, add items, and list stored events.

## Documentation

See [docs/README.md](docs/README.md) for an architecture overview, request flow diagram, and details about the automatic database seed that runs on startup.
