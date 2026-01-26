# Devices Registry (ASP.NET Web API + PostgreSQL)

Ops-ready Web API for registering devices/services and preparing the foundation for heartbeats, stale/offline reports, CI/CD, and GitOps.  
This project is built as a portfolio-friendly DevOps-adjacent example: database migrations, containerization, and clean API contracts.

## Highlights (R0)
- ASP.NET Web API (.NET 8)
- PostgreSQL via Docker Compose
- Entity Framework Core (migrations)
- DTO-based API contract + input validation
- Correct HTTP semantics (201 Created + Location)
- ProblemDetails + global exception handling

## Tech Stack
- .NET 8
- EF Core 8.x
- PostgreSQL (Docker image)
- Swagger/OpenAPI

## API Endpoints (R0)
- `GET /api/devices` — list devices
- `GET /api/devices/{id}` — get device by id
- `POST /api/devices` — create device (201 Created)

Planned (R1+):
- `POST /api/devices/{id}/heartbeat`
- `GET /api/reports/stale-devices?minutes=30`
- Health checks: `/health/live`, `/health/ready`
- Jenkins CI + GitOps (Argo CD)

## Prerequisites
- .NET SDK 8.x
- Docker Desktop
- (Optional) `dotnet-ef` tool

## Quick Start (Local)
### 1) Start PostgreSQL
docker compose up -d
### 2) Configure connection string
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=devices;Username=app;Password=app"
  }
}
### 3) Apply database migrations
dotnet tool install --global dotnet-ef
dotnet ef database update

### 4) Run the API
dotnet run

### 5) Open Swagger UI

https://localhost:<port>/swagger

## Example requests
### Create a device:
curl -X POST https://localhost:<port>/api/devices \
  -H "Content-Type: application/json" \
  -d '{"name":"node-01","status":"Online"}'
### List devices:
curl https://localhost:<port>/api/devices
### Get by id:
curl https://localhost:<port>/api/devices/1

## Project Structure
/Controllers
/Contracts
/Data
/Models
/Services
docker-compose.yml
README.md