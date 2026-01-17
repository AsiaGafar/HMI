#!/bin/bash
set -e

# Configuration
AWS_REGION="us-east-1"
KEY_NAME="dotspatial-key"
INSTANCE_TYPE="t3.medium"
APP_NAME="dotspatial"

echo "🚀 Installing DotSpatial on AWS EC2..."

# Step 1: Create key pair
echo "📝 Creating SSH key pair..."
aws ec2 create-key-pair \
  --key-name $KEY_NAME \
  --query 'KeyMaterial' \
  --output text > $KEY_NAME.pem 2>/dev/null || echo "Key already exists"
chmod 400 $KEY_NAME.pem

# Step 2: Create security group
echo "🔒 Setting up security group..."
SG_ID=$(aws ec2 create-security-group \
  --group-name ${APP_NAME}-sg \
  --description "DotSpatial RDP access" \
  --query 'GroupId' \
  --output text 2>/dev/null || \
  aws ec2 describe-security-groups \
  --group-names ${APP_NAME}-sg \
  --query 'SecurityGroups[0].GroupId' \
  --output text)

# Allow RDP access
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 3389 \
  --cidr 0.0.0.0/0 2>/dev/null || echo "RDP rule already exists"

# Step 3: Launch Windows Server instance
echo "🖥️  Launching Windows Server instance..."
INSTANCE_ID=$(aws ec2 run-instances \
  --image-id resolve:ssm:/aws/service/ami-windows-latest/Windows_Server-2022-English-Full-Base \
  --instance-type $INSTANCE_TYPE \
  --key-name $KEY_NAME \
  --security-group-ids $SG_ID \
  --tag-specifications "ResourceType=instance,Tags=[{Key=Name,Value=$APP_NAME}]" \
  --user-data file://install-dotspatial.ps1 \
  --query 'Instances[0].InstanceId' \
  --output text)

echo "⏳ Waiting for instance to start (this takes 3-5 minutes)..."
aws ec2 wait instance-running --instance-ids $INSTANCE_ID

# Get instance details
PUBLIC_IP=$(aws ec2 describe-instances \
  --instance-ids $INSTANCE_ID \
  --query 'Reservations[0].Instances[0].PublicIpAddress' \
  --output text)

echo ""
echo "✅ EC2 Instance launched successfully!"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "Instance ID: $INSTANCE_ID"
echo "Public IP: $PUBLIC_IP"
echo ""
echo "⏳ Wait 5 minutes for Windows to initialize, then:"
echo ""
echo "1. Get Windows password:"
echo "   aws ec2 get-password-data --instance-id $INSTANCE_ID --priv-launch-key-file $KEY_NAME.pem --query 'PasswordData' --output text | base64 -d"
echo ""
echo "2. Connect via RDP:"
echo "   Host: $PUBLIC_IP"
echo "   Username: Administrator"
echo ""
echo "3. DotSpatial will be installed at: C:\\DotSpatial"
echo ""
echo "To terminate instance:"
echo "   aws ec2 terminate-instances --instance-ids $INSTANCE_ID"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
