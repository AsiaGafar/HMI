#!/bin/bash
# OpenMU is already monolithic - this just runs the official image

echo "🚀 Running OpenMU (Already Monolithic)..."

docker run -d \
  --name openmu-monolithic \
  --restart unless-stopped \
  -p 80:8080 \
  -p 44405:44405 \
  -p 44406:44406 \
  -p 55901:55901 \
  -p 55902:55902 \
  -p 55903:55903 \
  -p 55904:55904 \
  -p 55905:55905 \
  -p 55906:55906 \
  -p 55980:55980 \
  -e DB_HOST=database \
  munique/openmu:latest

echo "✅ OpenMU Monolithic running!"
echo "Admin Panel: http://localhost"
echo "All services run in single process"