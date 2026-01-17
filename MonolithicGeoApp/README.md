# Monolithic Geospatial Application

A true monolithic .NET desktop GIS application with embedded geospatial data and UI.

## Features
- ✅ Single executable application
- ✅ Embedded geospatial data (cities with coordinates)
- ✅ Interactive map visualization
- ✅ Distance calculations using NetTopologySuite
- ✅ Add/remove locations dynamically
- ✅ WinForms UI interface
- ✅ No external services or databases

## Architecture
**Monolithic** - All code in single project:
- UI (WinForms)
- Business logic (distance calculations)
- Data (embedded city list)
- Rendering (map drawing)

## Build & Run

```bash
cd /workspaces/HMI/MonolithicGeoApp
dotnet restore
dotnet build
dotnet run
```

## Usage
1. Select 2 cities from the list
2. Click "Calculate Distance" to see distance between them
3. Click "Add Random City" to add new locations
4. Map shows all cities with connections between selected ones

## Technologies
- .NET 8.0
- WinForms
- NetTopologySuite (geometry operations)
- GeoJSON support

## Deploy to AWS
```bash
dotnet publish -c Release -r linux-x64 --self-contained
# Then use the AWS deployment scripts from parent directory
```
