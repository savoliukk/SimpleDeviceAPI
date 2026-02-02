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

## Local cluster + Argo CD + first sync
### Local cluster (kind)
```bash
kind create cluster --name simpledevice
kubectl cluster-info --context kind-simpledevice
```

### Install Argo CD
```bash
kubectl create namespace argocd
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml
```

### Access Argo CD UI
```bash
kubectl -n argocd port-forward svc/argocd-server 8081:80
```
Open http://localhost:8081

Get the initial admin password:
```bash
kubectl -n argocd get secret argocd-initial-admin-secret \
  -o jsonpath="{.data.password}" | base64 -d && echo
```

### Create the application and first sync
```bash
kubectl apply -f deploy/argocd-application.yaml
```
Then in the UI (or via CLI), Sync the `simpledevice-api-dev` app.

Expected state after first sync:
- Application shows **Synced** and **Healthy**
- Deployment has 1/1 ready pod

> Note: update `spec.source.repoURL` in `deploy/argocd-application.yaml` to point at your Git repository before syncing.

## K8s Postgres + migrations (dev overlay)
### Install Postgres (Bitnami Helm)
```bash
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install postgres bitnami/postgresql \
  --set auth.username=app \
  --set auth.password=app \
  --set auth.database=devices
```
The service name for this chart is `postgres-postgresql`, which is referenced in `deploy/overlays/dev/secret.yaml`.

### Build and load images into kind
```bash
docker build -t simpledevice-api:latest .
docker build -t simpledevice-api:migrations --target migrations .
kind load docker-image simpledevice-api:latest --name simpledevice
kind load docker-image simpledevice-api:migrations --name simpledevice
```

### Apply dev overlay (includes migrations Job)
```bash
kubectl apply -k deploy/overlays/dev
```

### Validate
```bash
kubectl get pods
kubectl logs job/simpledevice-ef-migrations
kubectl port-forward svc/simpledevice-api 8080:80
```
- Pods are Ready (deployment + job completed)
- Swagger UI: http://localhost:8080/swagger
- POST/GET persist data:
```bash
curl -X POST http://localhost:8080/api/devices \
  -H "Content-Type: application/json" \
  -d '{"name":"node-02","status":"Online"}'
curl http://localhost:8080/api/devices
```

## Demo steps (5 min)
1) `kind create cluster --name simpledevice`
2) `helm repo add bitnami https://charts.bitnami.com/bitnami && helm repo update`
3) `helm install postgres bitnami/postgresql --set auth.username=app --set auth.password=app --set auth.database=devices`
4) `docker build -t simpledevice-api:latest .`
5) `docker build -t simpledevice-api:migrations --target migrations .`
6) `kind load docker-image simpledevice-api:latest --name simpledevice`
7) `kind load docker-image simpledevice-api:migrations --name simpledevice`
8) `kubectl apply -k deploy/overlays/dev`
9) `kubectl port-forward svc/simpledevice-api 8080:80`
10) Open http://localhost:8080/swagger

Optional: capture Argo CD UI status after sync (screenshot or note Synced/Healthy state).

## Project Structure
/Controllers
/Contracts
/Data
/Models
/Services
docker-compose.yml
README.md
