#!/bin/bash

# Build and run .NET projects

echo "Select a project to run:"
echo "1) GeoCountriesApp (Blazor Web App)"
echo "2) MonolithicGeoApp.Web (ASP.NET Web App)"
echo "3) MonolithicGeoApp (Console App)"
echo "4) MonolithicRoverApp (Console App)"
echo "5) DotSpatialApp (WinForms - requires X11)"

read -p "Enter choice [1-5]: " choice

case $choice in
    1)
        cd GeoCountriesApp
        dotnet restore
        dotnet build
        dotnet run
        ;;
    2)
        cd MonolithicGeoApp.Web
        dotnet restore
        dotnet build
        dotnet run
        ;;
    3)
        cd MonolithicGeoApp
        dotnet restore
        dotnet build
        dotnet run
        ;;
    4)
        cd MonolithicRoverApp
        dotnet restore
        dotnet build
        dotnet run
        ;;
    5)
        cd DotSpatialApp
        dotnet restore
        dotnet build
        dotnet run
        ;;
    *)
        echo "Invalid choice"
        exit 1
        ;;
esac
