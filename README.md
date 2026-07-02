# Vehicle Explorer

A web application to select a car make and model year, then view the available
vehicle types and models for that selection. Data comes from the NHTSA vPIC API.

## Tech Stack

- Backend: .NET 10, Carter, MediatR, FluentValidation, HybridCache
- Frontend: Angular 19, Tailwind CSS
- Docker + Docker Compose

## Data Source

- Get all makes: `https://vpic.nhtsa.dot.gov/api/vehicles/getallmakes?format=json`
- Get vehicle types for make: `https://vpic.nhtsa.dot.gov/api/vehicles/GetVehicleTypesForMakeId/{makeId}?format=json`
- Get models for make, year and type: `https://vpic.nhtsa.dot.gov/api/vehicles/GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}?format=json`

## Prerequisites

- Docker Desktop (with Docker Compose)

## Run Locally (Docker)

1. Clone the repository:

   ```bash
   git clone https://github.com/osamaraed53/vehicle-explorer.git 
   cd vehicle-explorer
   ```

2. Create the env file:

   ```bash
   cp .env.example .env
   ```

3. Build and start:

   ```bash
   docker compose up --build
   ```

4. Open:

   - Web: http://localhost:4200
   - API health: http://localhost:8080/health

To stop:

```bash
docker compose down
```

## Run Locally (without Docker)

Backend:

```bash
cd backend/VehicleExplorer
dotnet run --project VehicleExplorer.Api
```

API runs on http://localhost:5103 (API reference at http://localhost:5103/scalar).

Frontend:

```bash
cd frontend/vehicle-explorer
npm install
ng serve 
```

Web runs on http://localhost:4200.

> For this mode, set `apiBaseUrl` in
> `frontend/vehicle-explorer/src/environments/environment.development.ts`
> to `http://localhost:5103`.

## Environment Variables

Set in `.env` (see `.env.example`):

- `API_PORT` - host port for the API (default 8080)
- `WEB_PORT` - host port for the web app (default 4200)
- `API_BASE_URL` - URL the browser uses to call the API (baked into the frontend at build time)
- `WEB_DOMAIN` - allowed CORS origin for the API (the web app URL)

## Deployment

-----

## Project Structure

```
backend/VehicleExplorer/VehicleExplorer.Api    .NET API
frontend/vehicle-explorer                       Angular app
docker-compose.yml                              Runs api + web together
```
