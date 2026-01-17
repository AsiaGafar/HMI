# OpenMU Monolithic Deployment - Quick Start

## Files Created:
- `Dockerfile.monolithic` - Docker image definition
- `appsettings.monolithic.json` - Configuration file
- `startup-monolithic.sh` - Service startup script
- `deploy-monolithic.sh` - AWS deployment script
- `user-data-monolithic.sh` - EC2 user data script
- `deploy-local.sh` - Local testing script

## Quick Deployment Options:

### Option 1: Local Testing (Recommended First)
```bash
./deploy-local.sh
```

### Option 2: AWS EC2 Deployment
```bash
# Make sure AWS CLI is configured
aws configure

# Deploy to AWS
./deploy-monolithic.sh
```

### Option 3: Manual Docker Build
```bash
# Clone OpenMU
git clone https://github.com/MUnique/OpenMU.git
cd OpenMU

# Copy monolithic files
cp ../Dockerfile.monolithic .
cp ../appsettings.monolithic.json .
cp ../startup-monolithic.sh .

# Build and run
docker build -f Dockerfile.monolithic -t openmu-monolithic .
docker run -p 80:80 -p 44405-44406:44405-44406 -p 55901-55906:55901-55906 -p 55980:55980 openmu-monolithic
```

## Access Points:
- Admin Panel: http://[IP_ADDRESS]
- Connect Server: [IP_ADDRESS]:44405
- Login Server: [IP_ADDRESS]:44406
- Game Servers: [IP_ADDRESS]:55901-55906
- Chat Server: [IP_ADDRESS]:55980

## Monitoring:
```bash
# Check container status
docker ps

# View logs
docker logs openmu-mono

# Check resource usage
docker stats openmu-mono
```

## Troubleshooting:
```bash
# Restart container
docker restart openmu-mono

# Access container shell
docker exec -it openmu-mono /bin/bash

# Check running processes
docker exec openmu-mono ps aux
```