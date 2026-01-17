#!/bin/bash
set -e

AWS_REGION=${AWS_REGION:-us-east-1}
KEY_NAME="dotspatial-key"
INSTANCE_TYPE="t3.medium"

echo "🚀 Deploying DotSpatial on AWS EC2..."

# Create key pair
aws ec2 create-key-pair --key-name $KEY_NAME --query 'KeyMaterial' --output text > $KEY_NAME.pem
chmod 400 $KEY_NAME.pem

# Create security group
SG_ID=$(aws ec2 create-security-group \
  --group-name dotspatial-sg \
  --description "DotSpatial RDP access" \
  --region $AWS_REGION \
  --query 'GroupId' --output text)

# Allow RDP
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 3389 \
  --cidr 0.0.0.0/0 \
  --region $AWS_REGION

# Launch Windows Server instance
INSTANCE_ID=$(aws ec2 run-instances \
  --image-id resolve:ssm:/aws/service/ami-windows-latest/Windows_Server-2022-English-Full-Base \
  --instance-type $INSTANCE_TYPE \
  --key-name $KEY_NAME \
  --security-group-ids $SG_ID \
  --region $AWS_REGION \
  --query 'Instances[0].InstanceId' \
  --output text)

echo "⏳ Waiting for instance to start..."
aws ec2 wait instance-running --instance-ids $INSTANCE_ID --region $AWS_REGION

# Get public IP
PUBLIC_IP=$(aws ec2 describe-instances \
  --instance-ids $INSTANCE_ID \
  --region $AWS_REGION \
  --query 'Reservations[0].Instances[0].PublicIpAddress' \
  --output text)

echo "✅ EC2 Instance launched!"
echo "Instance ID: $INSTANCE_ID"
echo "Public IP: $PUBLIC_IP"
echo ""
echo "Get Windows password:"
echo "aws ec2 get-password-data --instance-id $INSTANCE_ID --priv-launch-key-file $KEY_NAME.pem --region $AWS_REGION"
echo ""
echo "Connect via RDP to: $PUBLIC_IP"
