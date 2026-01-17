# Monolithic Rover Tracking Application

A TRUE monolithic .NET geospatial application converted from the multi-project Foss4gWorkshopDotNet.

## What Changed

### Original (Multi-project):
- 7 separate projects
- External PostgreSQL/GeoPackage database
- Separate RoverSimulator console app
- Multiple class libraries
- Complex dependency injection

### Monolithic (Single file):
- ✅ 1 project, 1 file (Program.cs)
- ✅ In-memory data storage (no external DB)
- ✅ Embedded rover simulation
- ✅ All logic in one place
- ✅ Embedded HTML UI with Leaflet maps
- ✅ Real-time tracking with auto-refresh

## Features
- 3 rovers with real-time tracking
- Interactive Leaflet map with trails
- Wind measurements (speed & direction)
- Simulate movement button
- Auto-simulate every 5 seconds
- In-memory data storage
- All code in 250 lines

## Run

```bash
cd /workspaces/HMI/MonolithicRoverApp
dotnet run
```

Open browser: http://localhost:5000

## Architecture
**TRUE MONOLITHIC:**
- Single Program.cs file
- No external dependencies (except NuGet packages)
- No database
- No separate services
- All UI, logic, and data in one file

## Deploy to AWS
```bash
dotnet publish -c Release
# Use AWS deployment scripts from parent directory
```
