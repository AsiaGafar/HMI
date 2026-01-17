# Quick Start: Install DotSpatial on AWS

## Prerequisites
- AWS account
- AWS CLI installed ✅
- AWS credentials configured

## Step 1: Configure AWS Credentials
```bash
aws login
# OR
aws configure
```

## Step 2: Run Installation Script
```bash
cd /workspaces/HMI
./install-dotspatial-aws.sh
```

## Step 3: Wait 5 Minutes
The script will:
- Create EC2 Windows Server instance
- Auto-install Git, .NET 8, and DotSpatial
- Provide connection details

## Step 4: Get Windows Password
```bash
# Copy the command from script output, or use:
aws ec2 get-password-data \
  --instance-id <INSTANCE_ID> \
  --priv-launch-key-file dotspatial-key.pem \
  --query 'PasswordData' \
  --output text | base64 -d
```

## Step 5: Connect via RDP
- Host: `<PUBLIC_IP>` (from script output)
- Username: `Administrator`
- Password: (from Step 4)

## Step 6: Access DotSpatial
- Location: `C:\DotSpatial`
- Desktop shortcut created automatically

## Cleanup (Stop Charges)
```bash
aws ec2 terminate-instances --instance-ids <INSTANCE_ID>
```

## Cost
- ~$0.04/hour (t3.medium)
- ~$30/month if running 24/7

## Troubleshooting
If script fails:
```bash
# Check AWS credentials
aws sts get-caller-identity

# Check region
export AWS_REGION=us-east-1

# Re-run script
./install-dotspatial-aws.sh
```
