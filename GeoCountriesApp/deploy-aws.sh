#!/bin/bash
set -e

AWS_REGION=${AWS_REGION:-us-east-1}
APP_NAME="geocountries"

echo "🚀 Deploying Geo Countries App to AWS..."

ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
ECR_URI="$ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$APP_NAME"

aws ecr create-repository --repository-name $APP_NAME --region $AWS_REGION 2>/dev/null || true

aws ecr get-login-password --region $AWS_REGION | docker login --username AWS --password-stdin $ECR_URI

docker build -t $APP_NAME:latest .
docker tag $APP_NAME:latest $ECR_URI:latest
docker push $ECR_URI:latest

aws apprunner create-service \
  --service-name $APP_NAME \
  --source-configuration "ImageRepository={ImageIdentifier=$ECR_URI:latest,ImageRepositoryType=ECR,ImageConfiguration={Port=8080}},AutoDeploymentsEnabled=true" \
  --instance-configuration "Cpu=1 vCPU,Memory=2 GB" \
  --region $AWS_REGION 2>/dev/null || \
aws apprunner update-service \
  --service-arn $(aws apprunner list-services --region $AWS_REGION --query "ServiceSummaryList[?ServiceName=='$APP_NAME'].ServiceArn" --output text) \
  --source-configuration "ImageRepository={ImageIdentifier=$ECR_URI:latest,ImageRepositoryType=ECR,ImageConfiguration={Port=8080}}" \
  --region $AWS_REGION

echo "✅ Deployment complete! Check AWS App Runner console for URL."
