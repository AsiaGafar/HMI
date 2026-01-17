# Use Windows Server Core as base
FROM mcr.microsoft.com/dotnet/runtime:8.0-windowsservercore-ltsc2022

WORKDIR /app

# Copy published application
COPY bin/Release/net8.0-windows/publish/ .

# Expose RDP port (if needed)
EXPOSE 3389

# Run the application
ENTRYPOINT ["DotSpatialApp.exe"]
