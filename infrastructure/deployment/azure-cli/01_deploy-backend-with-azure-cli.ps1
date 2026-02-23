$resourceGroup = "bootcamp"
$backendAppName = "pomodoro123"
$dbConnectionString = "Server=pomodoro1.database.windows.net,1433;Initial Catalog=pomodoro;Persist Security Info=False;User ID=<your_username>;Password=<your_password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
$jwtSecret = "4355b975e2cdde18c4184adaf3635359"
$allowedOrigin = "https://blue-bay-0e7e73c10.3.azurestaticapps.net"
$swaggerUrl = "https://pomodoro123-f5cvbveudfc2f7ah.centralus-01.azurewebsites.net/swagger"

# Install Azure CLI on Windows

winget install --exact --id Microsoft.AzureCLI --version 2.67.0

az login

# Choose your subscription

az account show

# Verify your active subscription (id)

# Login to azure portal : https://portal.azure.com/

# Create a Web app with these parameters

# - Publish : Code
# - Runtime stack .NET 9
# - Operating System : Linux
# - Pricing Plan : Free F1
# - Enable public access
# - Enable Application Insights

# Create the web app service first in Azure to retrieve the app name 

# Set the database connection string (replace <username> and <password> with your actual values)
az webapp config connection-string set --resource-group $resourceGroup --name $backendAppName --settings DatabaseConnectionString="$dbConnectionString" --connection-string-type SQLAzure

# Set the JWT Secret
az webapp config appsettings set --resource-group $resourceGroup --name $backendAppName --settings JwtSecret="$jwtSecret"

# Set allowed origins for CORS
az webapp config appsettings set --resource-group $resourceGroup --name $backendAppName --settings AllowedOrigins__0="$allowedOrigin"

# Build the Application
Set-Location backend\PomodoroTimer.Api
dotnet publish -c Release -o ./publish

# Create a zip file for deployment
Compress-Archive -Path ./publish/* -DestinationPath ./app.zip -Force

# Deploy to azure
az webapp deploy --resource-group $resourceGroup --name $backendAppName --src-path ./app.zip --type zip

# Check deployment status
az webapp show --resource-group $resourceGroup --name $backendAppName --query "state" --output tsv

# Browse to the Swagger UI
Start-Process $swaggerUrl