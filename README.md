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
- Health checks: `/health/live`, `/health/ready`
- GitOps scaffolding with Kustomize

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
- Jenkins CI + GitOps (Argo CD)

## Prerequisites
- .NET SDK 8.x
- Docker Desktop
- (Optional) `dotnet-ef` tool

## Quick Start (Local - Docker Compose)
### 1) Build and run Postgres + API
```bash
docker compose up -d --build
```
### 2) Health endpoints
- Liveness: http://localhost:8080/health/live
- Readiness (DB check): http://localhost:8080/health/ready

### 3) Open Swagger UI
http://localhost:8080/swagger

### Optional: local EF migrations against compose Postgres
```bash
dotnet tool install --global dotnet-ef
ConnectionStrings__Default="Host=localhost;Port=15432;Database=devices;Username=app;Password=app" \
  dotnet ef database update
```

## Quick Start (Local - dotnet run)
```bash
dotnet tool install --global dotnet-ef
cp appsettings.Development.example.json appsettings.Development.json
dotnet ef database update
dotnet run
```

## Example requests
### Create a device:
curl -X POST http://localhost:8080/api/devices \
  -H "Content-Type: application/json" \
  -d '{"name":"node-01","status":"Online"}'
### List devices:
curl http://localhost:8080/api/devices
### Get by id:
curl http://localhost:8080/api/devices/1

## Deploy (GitOps scaffolding)
Kustomize manifests live under `deploy/`:
- `deploy/base`: base Deployment, Service, ConfigMap, and Secret
- `deploy/overlays/dev`: dev overlay used by Argo CD later

Argo CD will point to `deploy/overlays/dev` when we wire up GitOps.

## Project Structure
/Controllers
/Contracts
/Data
/Models
/Services
docker-compose.yml
README.md
