$resourceGroup = "bootcamp"
$frontendAppName = "pomodoro1234"
$location = "CentralUS"
$sku = "Free"
$frontendPath = "frontend"
$buildDir = "dist"
$backendUrl = "https://pomodoro123-f5cvbveudfc2f7ah.centralus-01.azurewebsites.net"

# 1. Deploy the backend API 

# 2. Start from root directory

# 3. Update .env file with the correct API URL with the deployed backend URL

Set-Location $frontendPath

# Replace VITE_API_URL in .env file with the actual backend URL
(Get-Content .env.production) -replace 'VITE_API_BASE_URL=.*', "VITE_API_BASE_URL=$backendUrl" | Set-Content .env.production

Remove-Item -Recurse -Force .\dist

npm run build

# Go back to root directory
Set-Location ..

$buildPath = Join-Path $frontendPath $buildDir

# Create static web app if it doesn't exist
$existingApp = az staticwebapp show --name $frontendAppName --resource-group $resourceGroup 2>$null
if (-not $existingApp) {
    az staticwebapp create --name $frontendAppName --resource-group $resourceGroup --location $location --sku $sku --output none
}

# 4. Deploy the built files using Static Web Apps CLI
$deploymentToken = az staticwebapp secrets list --name $frontendAppName --resource-group $resourceGroup --query "properties.apiKey" -o tsv

# 5. Deploy the frontend
npx @azure/static-web-apps-cli deploy $buildPath --deployment-token $deploymentToken --env production
