# MPC Fabrics Tracker

ASP.NET Core MVC app that replaces the Excel **MPC Fabrics Project Plan** workbook for day-to-day tracking.

- **Tracker** — day-wise developer tasks (source of truth)
- **Capacity** — planned hours, leave, assigned hours, total tasks assigned (manual)
- **Summary** — spent hours, completed tasks, productivity %, utilization % calculated from Tracker

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Optional: SQL Server LocalDB (Windows) if you want SQL Server instead of SQLite

## Run locally

```bash
dotnet restore
dotnet run --launch-profile http
```

Open [http://localhost:5847](http://localhost:5847).

The first run creates the database and seeds developers, weeks, sample capacity, and tracker rows from the Excel plan.

## Database

### Default (SQLite — works on Windows/macOS/Linux)

`appsettings.json`:

```json
"Database": { "Provider": "Sqlite" },
"ConnectionStrings": {
  "DefaultConnection": "Data Source=MpcFabricsTracker.db"
}
```

### SQL Server LocalDB (Windows)

1. Copy settings from `appsettings.SqlServer.json` into `appsettings.Development.json`, or set:

```json
"Database": { "Provider": "SqlServer" },
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MpcFabricsTracker;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

2. Run the app. Tables are created automatically on startup.

## How metrics are calculated

| Metric | Rule |
|--------|------|
| Spent Hrs | Sum of Tracker `SpendHours` where `% Completed > 0` |
| Completed Task | Count of Tracker rows where `% Completed >= 1` |
| Productivity % | `Completed Task / Total Task Assigned × 100` |
| Utilization % | `Spent Hrs / (Planned Hrs − Leave Hrs) × 100` |
| Status | Completed / In Progress / Not Started from `% Completed` |

## Main screens

| URL | Purpose |
|-----|---------|
| `/Summary` | Developer × week dashboard |
| `/Tracker` | Add / edit / filter tasks |
| `/Capacity` | Edit manual assigned hours & task counts |

## Project layout

```
Controllers/   Summary, Tracker, Capacity
Models/        Developer, Week, TrackerEntry, WeeklyCapacity
Data/          AppDbContext, DbInitializer
Services/      SummaryCalculatorService
Views/         MVC Razor pages
```
