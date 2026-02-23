# Deploy Locally with VS Code
This guide explains how to run the Pomodoro Timer application locally using VS Code.

## Prerequisites ✅

- VS Code installed
- .NET 9.0 SDK installed
- Node.js installed (for frontend)
- A local SQL Server Database or remote Azure SQL Database

## Quick Start 🏃

### 1. Configure Database Connection

Before starting the application, update the database connection string:

1. Open `backend/PomodoroTimer.Api/appsettings.json`
2. Update the `DatabaseConnectionString` with your database credentials

### 2. Backend API (Port 5173)

1. Open the workspace in VS Code
2. Go to **Run and Debug** panel
3. Select **"Pomodoro Api"** configuration
4. The API will start on `http://localhost:5173`
5. Swagger UI will open automatically at `http://localhost:5173/swagger`

### 3. Frontend (Port 5000)

1. Open the workspace in VS Code
2. Go to **Run and Debug** panel
3. Select **"Pomodoro Frontend: Full Debug"** configuration
4. The API will start on `http://localhost:5000`

## Configuration 🔧

### Backend Configuration

The backend is configured in `backend/PomodoroTimer.Api/appsettings.json`:
- **Port**: 5173
- **CORS**: Allows requests from `http://localhost:5000`
- **Database**: Azure SQL Database connection string
- **JWT Secret**: Authentication key (change in production!)

### Frontend Configuration

The frontend is configured in `frontend/.env`:
- **API Base URL**: `http://localhost:5173` (points to the backend API)
- **Dev Server Port**: 5000 (configured in `vite.config.ts`)

## Verification ✓

Once both services are running:

1. **Backend**: Visit `http://localhost:5173/swagger` to see the API documentation
2. **Frontend**: Visit `http://localhost:5000` to use the application
3. The frontend should successfully communicate with the backend API

## Troubleshooting 🔍

**Port Already in Use**
- Make sure no other application is using ports 5000 or 5173
- Stop the processes and restart

**CORS Errors**
- Verify the frontend is running on port 5000
- Check that `AllowedOrigins` in `appsettings.json` includes `http://localhost:5000`

**API Connection Failed**
- Ensure the backend is running first
- Check that `VITE_API_BASE_URL` in `frontend/.env` is set to `http://localhost:5173`

## Development Workflow 💻

1. Start the backend API first (using F5 in VS Code)
2. Start the frontend dev server in a terminal
3. Make changes to your code
4. Frontend: Hot reload is automatic
5. Backend: Stop and restart the debugger to see changes