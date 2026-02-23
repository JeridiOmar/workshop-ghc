# Pomodoro Timer API - Backend

ASP.NET Core Web API (.NET 9) for the Pomodoro Timer application.

## 🏗️ Architecture

- **Framework**: ASP.NET Core Web API (.NET 9)
- **Database**: Azure SQL Database with Dapper (micro-ORM)
- **Authentication**: JWT Bearer tokens with 6-digit PIN
- **Security**: JWT authentication only
- **Password Hashing**: BCrypt

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Azure SQL Database](https://azure.microsoft.com/services/sql-database/) (or SQL Server for local dev)
- Visual Studio 2022 / VS Code / Rider

### Installation

1. **Clone the repository**
   ```bash
   cd backend/PomodoroTimer.Api
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure connection string**
   
   Update `appsettings.json` or use User Secrets:
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DatabaseConnectionString" "Server=tcp:your-server.database.windows.net,1433;Initial Catalog=pomodoro_db;User ID=your-user;Password=your-password;Encrypt=True;"
   dotnet user-secrets set "JwtSecret" "your-super-secret-jwt-key-minimum-32-characters-long"
   ```

4. **Run database migrations**
   
   Create the database using the SQL scripts in `/sql/` folder:
   - Execute scripts in order: 01, 02, 03

5. **Run the application**
   ```bash
   dotnet run
   ```

   The API will be available at:
   - HTTPS: `https://localhost:7001`
   - HTTP: `http://localhost:5000`
   - Swagger: `https://localhost:7001/swagger`

### Example Request

```bash
curl -X GET "https://localhost:7001/api/settings" \
  -H "Authorization: Bearer your-jwt-token"
```

## 📡 API Endpoints

### Authentication (No JWT required)

- `POST /api/auth/register` - Create new profile
  ```json
  {
    "username": "John Doe",
    "pin": "123456"
  }
  ```

- `POST /api/auth/login` - Load existing profile
  ```json
  {
    "username": "John Doe",
    "pin": "123456"
  }
  ```

### Settings (JWT required)

- `GET /api/settings` - Get user settings
- `POST /api/settings` - Save user settings

### Sessions (JWT required)

- `POST /api/sessions` - Record a Pomodoro session
  ```json
  {
    "type": "pomodoro",
    "durationSeconds": 25,
    "isCompleted": true,
    "completedAt": "2025-11-18T10:30:00Z"
  }
  ```

- `GET /api/sessions?startDate=2025-11-01&endDate=2025-11-18&limit=50` - Get session history

### Statistics

- `GET /api/stats/summary` - Get user statistics (JWT required)
- `GET /api/stats/ranking?page=1&limit=50` - Get global ranking

## 🗄️ Database Schema

The API uses Dapper (micro-ORM) with raw SQL queries for optimal performance:

- **users**: User credentials (username, pin_hash)
- **user_settings**: User preferences and customization
- **pomodoro_sessions**: All sessions (completed and incomplete)

See `/sql/` folder for complete schema and setup scripts.

## 🔐 Security

- **JWT Authentication**: Validates Bearer token for protected endpoints
- **PIN Hashing**: BCrypt with automatic salt generation
- **CORS**: Configured for Static Web App origin
- **HTTPS**: Required in production

## 🧪 Development

### Run in Development Mode

```bash
dotnet run --environment Development
```

### Watch Mode (auto-reload)

```bash
dotnet watch run
```

### Run Tests

```bash
dotnet test
```

## 📦 NuGet Packages

- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT authentication
- `Dapper` - Lightweight micro-ORM for data access
- `Microsoft.Data.SqlClient` - SQL Server data provider
- `BCrypt.Net-Next` - Password hashing
- `System.IdentityModel.Tokens.Jwt` - JWT token generation
- `Swashbuckle.AspNetCore` - Swagger/OpenAPI

## 🚢 Deployment to Azure

### Prerequisites

- Azure App Service (Basic B1 or higher)
- Azure SQL Database

### Deploy using Azure CLI

```bash
# Build the project
dotnet publish -c Release -o ./publish

# Create zip package
cd publish
zip -r ../api.zip .
cd ..

# Deploy to Azure App Service
az webapp deployment source config-zip \
  --resource-group rg-pomodoro-bootcamp \
  --name app-pomodoro-api-bootcamp \
  --src api.zip
```

### Configure App Service Settings

Set these environment variables in Azure App Service Configuration:

```
DatabaseConnectionString=<your-azure-sql-connection-string>
ApiKey=<your-secure-api-key>
JwtSecret=<your-jwt-secret>
AllowedOrigins__0=https://your-static-web-app.azurestaticapps.net
```

## 🐛 Troubleshooting

### Database Connection Issues

- Verify Azure SQL firewall rules allow your IP
- Check connection string format
- Ensure database exists and tables are created

### Authentication Errors

- Check JWT secret length (minimum 32 characters)
- Ensure Authorization header format: `Bearer <token>`

### CORS Errors

- Add your frontend URL to `AllowedOrigins` in appsettings.json
- Verify CORS middleware is registered in Program.cs

## 📝 Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `DatabaseConnectionString` | Azure SQL connection string | `Server=tcp:...` |
| `JwtSecret` | JWT signing secret | `min-32-chars-secret` |
| `AllowedOrigins__0` | CORS allowed origin | `https://localhost:5173` |

## 🔗 Related Documentation

- [Product Requirements Document](../../docs/specifications/product-requierements.md)
- [Database Schema](../../sql/README.md)
- [Frontend README](../../frontend/README.md)

## 📄 License

This project is part of the bootcamp workshop.

---

**Last Updated**: November 18, 2025  
**Version**: 1.0  
**.NET Version**: 9.0
