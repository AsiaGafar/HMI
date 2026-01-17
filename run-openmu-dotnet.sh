#!/bin/bash
cd OpenMU/src
dotnet restore
dotnet build
cd Startup
dotnet run -- -demo -autostart
