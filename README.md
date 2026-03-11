# MinimalAPIs Template

A production-ready template for building **.NET 10 Minimal APIs** following **Clean Architecture**, **Domain-Driven Design (DDD)**, and **CQRS** — without any external mediator libraries.

---

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Design Patterns](#design-patterns)
- [Technologies](#technologies)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Adding a New Use Case](#adding-a-new-use-case)
- [License](#license)

---

## Overview

This repository provides a clean, minimal, and opinionated starting point for ASP.NET Core APIs. It demonstrates how to combine Minimal APIs with architectural best practices — keeping the codebase simple, testable, and easy to extend — without relying on third-party mediator packages.

---

## Architecture

The solution is organized into **3 layers**, each with a strict dependency rule: **outer layers depend on inner layers, never the other way around**.

The **Api** project has dependencies on both Infrastructure and Core.

The **Infrastructure** project has dependencies on Core.

The **Core** project has **zero dependencies** on Infrastructure or any framework-specific package.

---

## Design Patterns

### Clean Architecture

Each layer only references layers closer to the center. Concrete types (e.g., `AppDbContext`, `WeatherForecastRepository`) are never referenced by the `Core` project or the endpoint definitions — they are resolved through DI at startup.

### Domain-Driven Design (DDD)

- **Encapsulated entities**: `WeatherForecast` exposes `private set` properties and a static `Create(...)` factory method that enforces invariants at construction time.
- **Repository interface in the Domain**: `IWeatherForecastRepository` is owned by the domain layer, not by Infrastructure.
- **No anemic model**: entity creation logic lives inside the entity itself, not in a service.

### CQRS (without MediatR)

Commands and queries are fully separated using two lightweight generic interfaces defined in `Application/Abstractions`:
```csharp
public interface IQueryHandler<TQuery, TResult> { Task<TResult> HandleAsync(TQuery query); }
public interface ICommandHandler<TCommand, TResult> { Task<TResult> HandleAsync(TCommand command); }
```
Each use case is a single, focused class. Endpoints inject only the handler they need:
```csharp
// Query 
app.MapGet("/weatherforecast", async (IQueryHandler<GetAllWeatherForecastsQuery, IEnumerable<WeatherForecast>> handler) => Results.Ok(await handler.HandleAsync(new GetAllWeatherForecastsQuery())));
// Command 
app.MapPost("/weatherforecast", async (CreateWeatherForecastCommand command, ICommandHandler<CreateWeatherForecastCommand, WeatherForecast> handler) => { var created = await handler.HandleAsync(command); return Results.Created($"/weatherforecast/{created.Id}", created); });
```

### Dependency Injection via Extension Methods

Each layer registers its own services through dedicated extension methods, keeping `Program.cs` clean:
```csharp
builder.Services.AddApplication(); builder.Services.AddInfrastructure(builder.Configuration);
```
---

## Technologies

| Technology | Version | Purpose |

| [.NET](https://dotnet.microsoft.com/) | 10 | Runtime & SDK |  
| [ASP.NET Core Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis) | 10 | HTTP layer |  
| [Entity Framework Core](https://learn.microsoft.com/ef/core/) | 10 | ORM |  
| [SQL Server](https://www.microsoft.com/sql-server) | — | Relational database |  
| [Scalar](https://scalar.com/) | – | API documentation |

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- A running **SQL Server** instance (local or remote)
- [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet)
dotnet tool install --global dotnet-ef

---

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/RobertoFalconi/MinimalAPIsTemplate.git 
```
### 2. Configure the connection string

This is a project with a code-first approach. DB should be created on the first run. You can edit `MinimalAPIsAndCleanArchitecture/appsettings.json` to change the ConnectionString:
```json
{ "ConnectionStrings": { "DefaultConnection": "Server=YOUR_SERVER;Database=WeatherDb;Trusted_Connection=True;TrustServerCertificate=True;" } }
```

### 3. Apply database migrations
```bash
dotnet ef database update --project MinimalAPIsAndCleanArchitecture.Infrastructure --startup-project MinimalAPIsAndCleanArchitecture
```

### 4. Run the application
```bash
dotnet run --project MinimalAPIsAndCleanArchitecture
```
The **Scalar API Reference** opens automatically at `http://localhost:5179/scalar/v1` in the browser on startup (development mode).

---

## API Endpoints

| Method | Route | Auth required | Description | Request body |
|--------|-------|:---:|-------------|--------------|
| `POST` | `/auth/token` | ✗ | Returns a JWT Bearer token | `{ "username": "string", "password": "string" }` |
| `GET` | `/weatherforecast` | ✔ | Returns all weather forecasts | — |
| `POST` | `/weatherforecast` | ✔ | Creates a new weather forecast | `{ "date": "2026-03-04", "temperatureC": 22, "summary": "Warm" }` |

### Example — POST `/weatherforecast`

**Request**
```json
{ "date": "2026-03-04", "temperatureC": 22, "summary": "Warm" }
```
**Response** `201 Created`
```json
{ "id": 1, "date": "2026-03-04", "temperatureC": 22, "temperatureF": 71, "summary": "Warm" }
```

---

## Authentication

The API uses **JWT Bearer** authentication. Protected endpoints require a valid token in the `Authorization` header.

### 1. Obtain a token

Call `POST /auth/token` with valid credentials:

```http
POST /auth/token
Content-Type: application/json

{
  "username": "",
  "password": ""
}
```

**Response** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

> ⚠️ Demo credentials are `` / ``. Replace `GenerateTokenCommandHandler` with a real user store before going to production.

### 2. Call a protected endpoint

Add the token to every subsequent request via the `Authorization` header:

```http
GET /weatherforecast
Authorization: Bearer <token>
```

### 3. Using Scalar

1. Open `http://localhost:5179/scalar/v1`
2. Add "**Authorization**" in the headers with "Authorization" as key and paste the token in the value field with "Bearer " prefix

---

## Adding a New Use Case

The CQRS structure makes it straightforward to add new features without touching existing code (**Open/Closed Principle**).

**Example: delete a forecast by ID**

#### 1. Create the command
```csharp
// Core/Application/Commands/DeleteWeatherForecastCommand.cs 
public record DeleteWeatherForecastCommand(int Id);
```
#### 2. Extend the repository interface
```csharp
// Core/Domain/Interfaces/IWeatherForecastRepository.cs 
Task DeleteAsync(int id);
```
#### 3. Implement the handler
```csharp
// Core/Application/Commands/DeleteWeatherForecastCommandHandler.cs 
public class DeleteWeatherForecastCommandHandler(IWeatherForecastRepository repository) : ICommandHandler<DeleteWeatherForecastCommand, bool> { public async Task<bool> HandleAsync(DeleteWeatherForecastCommand command) { await repository.DeleteAsync(command.Id); return true; } }
```
#### 4. Register in DI
```csharp
// Core/DependencyInjection.cs 
services.AddScoped< ICommandHandler<DeleteWeatherForecastCommand, bool>, DeleteWeatherForecastCommandHandler>();
```
#### 5. Map the endpoint
```csharp
// Endpoints/WeatherForecastEndpoint.cs 
app.MapDelete("/weatherforecast/{id:int}", async (int id, ICommandHandler<DeleteWeatherForecastCommand, bool> handler) => { await handler.HandleAsync(new DeleteWeatherForecastCommand(id)); return Results.NoContent(); }).WithName("DeleteWeatherForecast");
```
---

## License

This project is licensed under the [MIT License](LICENSE).