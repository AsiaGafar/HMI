#!/bin/bash
# File: deploy-local.sh

echo "🚀 OpenMU is already monolithic - using official image..."

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker first."
    exit 1
fi

echo "🚀 Starting OpenMU Monolithic container..."
docker run -d \
  --name openmu-mono \
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
  munique/openmu:latest

echo "✅ OpenMU Monolithic deployed locally!"
echo "Admin Panel: http://localhost"
echo "All services run in single process - already monolithic!"
echo "Check logs: docker logs openmu-mono"
echo "Stop: docker stop openmu-mono"