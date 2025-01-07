# Clean Architecture with .NET 8

This project demonstrates the implementation of Clean Architecture principles using .NET 8. The solution follows a domain-centric approach with clear separation of concerns.

## Architecture Overview

The solution follows the Clean Architecture principles with 4 main layers:

1. **Domain Layer**: Contains enterprise-wide business rules, entities, and interfaces
2. **Application Layer**: Contains application-specific business rules and use cases
3. **Infrastructure Layer**: Contains implementations of interfaces defined in inner layers
4. **Presentation Layer**: Contains the API controllers and UI components

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- An IDE (Visual Studio 2022, VS Code, or JetBrains Rider)
- SQL Server (or your preferred database)

## Getting Started

1. Clone the repository
   ```bash
   git clone https://github.com/yourusername/clean_net_8.git
   cd clean_net_8
   ```

2. Restore dependencies
   ```bash
   dotnet restore
   ```

3. Update the database connection string in `src/WebApi/appsettings.json`

4. Apply database migrations
   ```bash
   cd src/WebApi
   dotnet ef database update
   ```

5. Run the application
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7001` (HTTP) and `http://localhost:7000` (HTTPS)

## Project Features

- Clean Architecture implementation
- CQRS with MediatR
- Entity Framework Core
- FluentValidation
- Swagger/OpenAPI documentation
- JWT Authentication
- Logging with Serilog
- Unit Testing with xUnit
- Integration Testing

## Best Practices

- Follow SOLID principles
- Keep the Domain layer independent of external concerns
- Use dependency injection
- Write unit tests for business logic
- Use CQRS pattern for separation of read and write operations
- Implement validation using FluentValidation
- Use repository pattern for data access

## Common Commands

Build the solution:
```bash
dotnet build

# Run unit tests
dotnet test

# Add a migration
cd src/Infrastructure
dotnet ef migrations add MigrationName -s ../WebApi
```
