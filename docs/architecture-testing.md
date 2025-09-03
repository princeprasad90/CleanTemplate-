# Architecture Testing Guidelines

This repository uses [NetArchTest](https://github.com/BenMorris/NetArchTest) and xUnit to enforce clean architecture boundaries.

## Location

Architecture tests live in `tests/CleanTemplate.ArchitectureTests`.

## Conventions

- Target .NET 8.0 and enable nullable reference types.
- Reference the `CleanTemplate.Domain`, `CleanTemplate.Application`, and `CleanTemplate.Infrastructure` projects to examine dependencies.
- Verify that:
  - Domain does not depend on Application or Infrastructure.
  - Application does not depend on Infrastructure.
- Use `NetArchTest.Rules` to express dependency rules.

## Running tests

Run all tests, including architecture checks, from the repository root:

```bash
dotnet test
```

## Adding new features

When new layers or projects are added, extend the architecture tests to validate their dependency rules and keep this document updated with any new guidelines.
