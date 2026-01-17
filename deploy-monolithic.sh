#!/bin/bash
# File: deploy-monolithic.sh

set -e

# Configuration
AWS_REGION="us-east-1"
KEY_NAME="openmu-mono-key"
INSTANCE_TYPE="t3.xlarge"  # 4 vCPU, 16GB RAM for monolithic
APP_NAME="openmu-monolithic"

echo "🚀 Deploying OpenMU Monolithic on AWS..."

# Create key pair
aws ec2 create-key-pair \
  --key-name $KEY_NAME \
  --query 'KeyMaterial' \
  --output text > $KEY_NAME.pem 2>/dev/null || echo "Key exists"
chmod 400 $KEY_NAME.pem

# Create security group
SG_ID=$(aws ec2 create-security-group \
  --group-name ${APP_NAME}-sg \
  --description "OpenMU Monolithic Server" \
  --query 'GroupId' \
  --output text 2>/dev/null || \
  aws ec2 describe-security-groups \
  --group-names ${APP_NAME}-sg \
  --query 'SecurityGroups[0].GroupId' \
  --output text)

# Single security group rule for all ports
aws ec2 authorize-security-group-ingress --group-id $SG_ID --protocol tcp --port 22 --cidr 0.0.0.0/0 2>/dev/null || true
aws ec2 authorize-security-group-ingress --group-id $SG_ID --protocol tcp --port 80 --cidr 0.0.0.0/0 2>/dev/null || true
aws ec2 authorize-security-group-ingress --group-id $SG_ID --protocol tcp --port 44405-44406 --cidr 0.0.0.0/0 2>/dev/null || true
aws ec2 authorize-security-group-ingress --group-id $SG_ID --protocol tcp --port 55901-55906 --cidr 0.0.0.0/0 2>/dev/null || true
aws ec2 authorize-security-group-ingress --group-id $SG_ID --protocol tcp --port 55980 --cidr 0.0.0.0/0 2>/dev/null || true

# Launch instance with monolithic user data
INSTANCE_ID=$(aws ec2 run-instances \
  --image-id ami-0c02fb55956c7d316 \
  --instance-type $INSTANCE_TYPE \
  --key-name $KEY_NAME \
  --security-group-ids $SG_ID \
  --user-data file://user-data-monolithic.sh \
  --tag-specifications "ResourceType=instance,Tags=[{Key=Name,Value=$APP_NAME}]" \
  --query 'Instances[0].InstanceId' \
  --output text)

echo "⏳ Waiting for instance to start..."
aws ec2 wait instance-running --instance-ids $INSTANCE_ID

PUBLIC_IP=$(aws ec2 describe-instances \
  --instance-ids $INSTANCE_ID \
  --query 'Reservations[0].Instances[0].PublicIpAddress' \
  --output text)

echo "✅ OpenMU Monolithic Server deployed!"
echo "Instance ID: $INSTANCE_ID"
echo "Public IP: $PUBLIC_IP"
echo "Admin Panel: http://$PUBLIC_IP"
echo "All services running on single instance"