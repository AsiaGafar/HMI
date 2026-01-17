# DotSpatial AWS Deployment Guide

## Current Setup
Your DotSpatialApp is a Windows Forms desktop application (.NET 8.0-windows).

## Deployment Options

### Option 1: AWS EC2 Windows Instance (Quick Deploy)
**Best for:** Direct deployment of existing desktop app

```bash
cd DotSpatialApp
chmod +x deploy-aws-ec2.sh
./deploy-aws-ec2.sh
```

**Steps after deployment:**
1. Connect via RDP to the EC2 instance
2. Install .NET 8.0 Desktop Runtime
3. Copy DotSpatialApp files to the instance
4. Run the application

**Cost:** ~$30-50/month (t3.medium instance)

---

### Option 2: AWS WorkSpaces (Enterprise Solution)
**Best for:** Multiple users, managed desktop environment

```bash
# Prerequisites: Set up AWS Directory Service first
aws ds create-directory --name corp.example.com --password <password> --size Small

# Then deploy WorkSpace
cd DotSpatialApp
chmod +x deploy-aws-workspaces.sh
./deploy-aws-workspaces.sh
```

**Cost:** ~$25-75/month per user

---

### Option 3: Convert to Blazor Web App (Recommended)
**Best for:** Scalable, cloud-native deployment

Use GeoBlazor (already in your project) instead of DotSpatial:

```bash
cd ../GeoBlazor
chmod +x deploy.sh
./deploy.sh
```

**Advantages:**
- No RDP needed - access via browser
- Auto-scaling
- Lower cost (~$10-20/month)
- Better for HMI/SCADA applications

---

## Quick Start: EC2 Deployment

### 1. Deploy EC2 Instance
```bash
cd /workspaces/HMI/DotSpatialApp
chmod +x deploy-aws-ec2.sh
./deploy-aws-ec2.sh
```

### 2. Build Application Locally
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

### 3. Upload to EC2
```bash
# Get instance IP from previous step
scp -i dotspatial-key.pem -r bin/Release/net8.0-windows/win-x64/publish/* Administrator@<PUBLIC_IP>:C:/DotSpatialApp/
```

### 4. Connect and Run
- RDP to the instance
- Navigate to C:/DotSpatialApp/
- Run DotSpatialApp.exe

---

## Alternative: AppStream 2.0 (Application Streaming)
Stream the desktop app to browsers without full desktop:

```bash
# Create AppStream image builder
aws appstream create-image-builder \
  --name dotspatial-builder \
  --instance-type stream.standard.medium \
  --image-name AppStream-WinServer2019-07-12-2023
```

**Cost:** Pay per streaming hour (~$0.20-0.40/hour)

---

## Recommendation
For an HMI project, I recommend **Option 3 (GeoBlazor)** because:
- Web-based access (no RDP)
- Modern architecture
- Better integration with microservices
- Lower operational cost
- Already in your project structure

Would you like help with any specific option?
