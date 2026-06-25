# ICMarkets WEB API .NET Developer Project

A .NET 8 Web API that fetches blockchain data from [BlockCypher](https://www.blockcypher.com/dev/bitcoin/) APIs (ETH, DASH, BTC mainnet, BTC testnet3, LTC), stores responses in SQLite with a `CreatedAt` timestamp, and exposes history endpoints plus HTTP request audit logs via Swagger.

## Architecture

The solution follows a layered (vertical slice) architecture:

| Layer | Project | Responsibility |
|-------|---------|----------------|
| API | `MyBlockchain.Api` | Controllers, filters, validation, Swagger, health checks |
| Application | `MyBlockchain.Application` | Services, DTOs, AutoMapper, interfaces |
| Infrastructure | `MyBlockchain.Infrastructure` | EF Core, repositories, unit of work |
| Domain | `MyBlockchain.Domain` | Entities |

**Design patterns used:** Repository, Unit of Work, Action Filter (API audit logging).

**Async/parallel patterns:** `async`/`await` throughout; `Task.WhenAll` for parallel multi-chain fetch via `POST /api/Blockchain/fetch-all`.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (optional, for containerized deployment)

## Getting Started

### Clone and build

```bash
git clone https://github.com/nmokariya25/blockchain_app.git
cd blockchain_app
dotnet restore
dotnet build
```

### Run locally

```bash
dotnet run --project src/MyBlockchain.Api/MyBlockchain.Api.csproj --launch-profile https
```

Open Swagger at [https://localhost:7218/swagger](https://localhost:7218/swagger) (or `http://localhost:5218/swagger` with the `http` profile).

### Apply database migrations

```bash
dotnet ef database update --project src/MyBlockchain.Infrastructure --startup-project src/MyBlockchain.Api
```

> If `dotnet-ef` is not installed: `dotnet tool install --global dotnet-ef`

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/EthBlock/fetch` | Fetch latest ETH block from BlockCypher and store |
| `GET` | `/api/EthBlock/history/{count?}` | ETH block history (newest `CreatedAt` first) |
| `POST` | `/api/DashBlock/fetch` | Fetch latest DASH block |
| `GET` | `/api/DashBlock/history/{count?}` | DASH block history |
| `POST` | `/api/BtcBlock/fetch` | Fetch latest BTC mainnet block |
| `GET` | `/api/BtcBlock/history/{count?}` | BTC block history |
| `POST` | `/api/BitCoinBlock/fetch` | Fetch latest BTC testnet3 block |
| `GET` | `/api/BitCoinBlock/history/{count?}` | BTC testnet3 block history |
| `POST` | `/api/LtcBlock/fetch` | Fetch latest LTC block |
| `GET` | `/api/LtcBlock/history/{count?}` | LTC block history |
| `POST` | `/api/Blockchain/fetch-all` | Fetch all chains in parallel (`Task.WhenAll`) |
| `GET` | `/api/ApiAudit/history/{count?}` | HTTP/HTTPS request audit log history |
| `GET` | `/health` | Health check (self + database) |
| `GET` | `/healthchecks` | Health check alias |

## Configuration

BlockCypher endpoints are configured in `appsettings.json`:

```json
"BlockCypherEndPoints": {
  "EthBlock": "https://api.blockcypher.com/v1/eth/main",
  "DashBlock": "https://api.blockcypher.com/v1/dash/main",
  "BtcBlock": "https://api.blockcypher.com/v1/btc/main",
  "BitCoinBlock": "https://api.blockcypher.com/v1/btc/test3",
  "LtcBlock": "https://api.blockcypher.com/v1/ltc/main"
}
```

SQLite connection string (default): `Data Source=block_cypher_data.db`

## Docker

### Build and run with Docker

```bash
cd src
docker build -t myblockchainapp -f MyBlockchain.Api/Dockerfile .
docker run -d -p 5000:8080 --name myblockchainapp myblockchainapp:latest
```

Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger)

### Docker Compose

```bash
docker compose up --build
```

## Running Tests

```bash
dotnet test
```

Test projects:

- `tests/MyBlockChain.Tests.Unit` — service unit tests
- `tests/MyBlockChain.Tests.Integration` — repository integration tests
- `tests/MyBlockChain.Tests.Functional` — API endpoint tests (health, audit, controllers)

## Launch Profiles

Defined in `src/MyBlockchain.Api/Properties/launchSettings.json`:

- **https** — HTTPS + HTTP with Swagger
- **http** — HTTP only on port 5218
- **Container (Dockerfile)** — Docker debug profile
- **IIS Express**

## Branch Strategy

- `main` — stable release branch
- `development` — active development branch

## Technology Stack

- .NET 8, ASP.NET Core Web API
- Entity Framework Core + SQLite
- AutoMapper, FluentValidation, NLog
- Swashbuckle (Swagger/OpenAPI)
- xUnit, Moq, `Microsoft.AspNetCore.Mvc.Testing`
