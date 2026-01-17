#!/bin/bash
cd OpenMU/src/Startup
sudo dotnet run -p:ci=true -- -demo -autostart -resolveIP:loopback
