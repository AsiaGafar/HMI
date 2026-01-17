# Deploy Geo Countries Data to AWS

## What This Does
- Displays `datasets/geo-countries` GeoJSON data on an interactive map
- .NET Blazor application with Leaflet UI
- Ready for AWS deployment

## Quick Deploy to AWS

### 1. Configure AWS (if not done)
```bash
aws configure
```

### 2. Deploy
```bash
cd /workspaces/HMI/GeoCountriesApp
chmod +x deploy-aws.sh
./deploy-aws.sh
```

### 3. Get URL
```bash
aws apprunner list-services --region us-east-1 --query "ServiceSummaryList[?ServiceName=='geocountries'].ServiceUrl" --output text
```

## Test Locally First
```bash
cd /workspaces/HMI/GeoCountriesApp
dotnet run
```
Open: http://localhost:5000/map

## What You'll See
- Interactive world map
- All countries from `datasets/geo-countries`
- Click countries for details
