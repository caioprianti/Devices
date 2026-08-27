# Devices API

REST API for persisting and managing device resources.

## Technologies

- .NET 10
- C#
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- FluentValidation
- Swagger/OpenAPI
- xUnit, Moq, and Testcontainers
- Docker and Docker Compose

## Architecture

The solution is divided into four projects:

- `Devices.Domain`: entities, states, domain rules, errors, and result types.
- `Devices.Application`: commands, queries, handlers, response models, and repository abstractions.
- `Devices.Infrastructure`: Entity Framework Core, PostgreSQL, migrations, and repository implementation.
- `Devices.Api`: HTTP endpoints, request validation, Swagger, error handling, and application startup.

The application uses lightweight CQRS. Each operation has a command or query and a directly injected handler. No mediator library is used.

## Domain

A device contains:

- `Id`
- `Name`
- `Brand`
- `State`
- `CreationTime`

Supported states:

- `available`
- `in-use`
- `inactive`

Domain rules:

- Creation time cannot be updated.
- Name and brand cannot be changed while a device is in use.
- A device in use cannot be deleted.

## Run With Docker Compose

Docker is the only prerequisite for this option.

```bash
docker compose up --build
```

The services will be available at:

- API: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`
- PostgreSQL: `localhost:5432`

The application applies pending Entity Framework Core migrations during startup.

Stop the services with:

```bash
docker compose down
```

To also remove the PostgreSQL volume:

```bash
docker compose down --volumes
```

## Run Locally

Requirements:

- .NET 10 SDK
- Docker

Start PostgreSQL:

```bash
docker compose up -d postgres
```

Run the API:

```bash
dotnet run --project Devices.Api/Devices.Api.csproj
```

Swagger opens automatically when using a local launch profile. It can also be accessed at the URL displayed by the application followed by `/swagger`.

The local development connection string is defined in `Devices.Api/appsettings.Development.json`. Other environments can provide it using:

```text
ConnectionStrings__Postgres
```

## Endpoints

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/devices` | Create a device |
| `GET` | `/devices/{id}` | Get a device by ID |
| `GET` | `/devices` | Get all devices |
| `GET` | `/devices?brand={brand}` | Filter devices by brand |
| `GET` | `/devices?state={state}` | Filter devices by state |
| `PUT` | `/devices/{id}` | Fully update a device |
| `PATCH` | `/devices/{id}` | Partially update a device |
| `DELETE` | `/devices/{id}` | Delete a device |

Brand and state filters can be combined.

## Request Examples

Create a device:

```json
{
  "name": "Test Name",
  "brand": "Test Brand",
  "state": "available"
}
```

Fully update a device:

```json
{
  "name": "Test Name 2",
  "brand": "Test Brand 2",
  "state": "inactive"
}
```

Partially update a device:

```json
{
  "state": "in-use"
}
```

## HTTP Responses

- `200 OK`: successful read or update.
- `201 Created`: device created successfully.
- `204 No Content`: device deleted successfully.
- `400 Bad Request`: invalid request or filter.
- `404 Not Found`: device not found.
- `409 Conflict`: operation rejected by a domain rule.
- `500 Internal Server Error`: unexpected application error.

Errors use the Problem Details format. Unexpected errors include a trace identifier that can be correlated with application logs.

## Tests

Run all tests:

```bash
dotnet test Devices.sln
```

Run only unit tests:

```bash
dotnet test tests/Devices.UnitTests/Devices.UnitTests.csproj
```

Run only integration tests:

```bash
dotnet test tests/Devices.IntegrationTests/Devices.IntegrationTests.csproj
```

Integration tests require Docker because they start a temporary PostgreSQL database with Testcontainers.

## Build

```bash
dotnet restore Devices.sln
dotnet build Devices.sln --configuration Release
```

## Future Improvements

- Add pagination to collection queries.
- Add metrics and distributed tracing.
