$resourceGroup = "bootcamp"
$backendAppName = "pomodoro123"
$allowedOrigin = "https://blue-bay-0e7e73c10.3.azurestaticapps.net"

# 1. Replace allowed origins by your frontend URL

# 2. Set allowed origins for CORS on the backend web app
az webapp config appsettings set --resource-group $resourceGroup --name $backendAppName --settings AllowedOrigins__0="$allowedOrigin"
