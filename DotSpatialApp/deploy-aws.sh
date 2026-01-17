#!/bin/bash
set -e

AWS_REGION=${AWS_REGION:-us-east-1}
DEPLOYMENT_TYPE=${1:-ec2}

echo "🚀 DotSpatial AWS Deployment"
echo "Deployment Type: $DEPLOYMENT_TYPE"
echo ""

case $DEPLOYMENT_TYPE in
  ec2)
    echo "Deploying to EC2 Windows Instance..."
    ./deploy-aws-ec2.sh
    ;;
  
  workspaces)
    echo "Setting up AWS WorkSpaces..."
    ./deploy-aws-workspaces.sh
    ;;
  
  appstream)
    echo "Deploying to AppStream 2.0..."
    
    # Build and publish
    dotnet publish -c Release -r win-x64 --self-contained
    
    # Create S3 bucket for AppStream
    BUCKET_NAME="dotspatial-appstream-$(date +%s)"
    aws s3 mb s3://$BUCKET_NAME --region $AWS_REGION
    
    # Upload application
    aws s3 sync bin/Release/net8.0-windows/win-x64/publish/ s3://$BUCKET_NAME/DotSpatialApp/
    
    echo "✅ Application uploaded to S3: $BUCKET_NAME"
    echo "Next: Create AppStream Image Builder and install from S3"
    ;;
  
  *)
    echo "Usage: ./deploy-aws.sh [ec2|workspaces|appstream]"
    exit 1
    ;;
esac

echo ""
echo "✅ Deployment initiated!"
