#!/bin/bash
set -e

AWS_REGION=${AWS_REGION:-us-east-1}

echo "🚀 Setting up AWS WorkSpaces for DotSpatial..."

# Create WorkSpaces directory
aws workspaces create-workspace \
  --workspaces \
    DirectoryId=d-xxxxxxxxxx \
    UserName=dotspatial-user \
    BundleId=wsb-clj85qzj1 \
    WorkspaceProperties={RunningMode=AUTO_STOP,RunningModeAutoStopTimeoutInMinutes=60} \
  --region $AWS_REGION

echo "✅ WorkSpace created. Install DotSpatial app via RDP."
