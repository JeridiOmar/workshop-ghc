# Pomodoro Timer Application

> Une application web complète pour gérer vos sessions de travail avec la technique Pomodoro.

---

## ℹ️ Description du projet

**Pomodoro Timer** est une application web full-stack permettant aux utilisateurs de :
- ⏱️ Gérer des sessions de travail selon la technique Pomodoro
- 📊 Consulter des statistiques détaillées (quotidiennes, hebdomadaires, mensuelles)
- 🏆 Voir un classement global des utilisateurs
- ⚙️ Personnaliser les paramètres des sessions (durée, pauses, etc.)
- 👤 Gérer leur profil utilisateur

**Stack technologique :**
- **Backend** : ASP.NET Core (.NET 6+) avec Entity Framework Core
- **Frontend** : Vue 3 + TypeScript + Vite + Tailwind CSS
- **Base de données** : SQL Server (structure fournie)
- **Infrastructure** : Déploiement sur Azure

---

## 🔧 Guide d'installation des dépendances

### Prérequis
- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
- [Node.js 16+](https://nodejs.org/) et npm
- [SQL Server](https://www.microsoft.com/sql-server) (local ou Azure)
- [Git](https://git-scm.com/)

### Installation backend

```bash
cd backend
dotnet restore
```

### Installation frontend

```bash
cd frontend
npm install
```

### Configuration de la base de données

1. Exécuter les scripts SQL :
```bash
cd sql/tables
# Exécuter dans votre instance SQL Server :
# 01_users.sql
# 02_user_settings.sql
# 03_pomodoro_sessions.sql
```

2. Configurer la chaîne de connexion dans `backend/PomodoroTimer.Api/appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PomodoroDb;User Id=SA;Password=YOUR_PASSWORD;"
  }
}
```

---

## 🏗️ Guide de compilation/build

### Backend

**Avec VS Code :**
```bash
cd backend
dotnet build PomodoroTimer.Api/PomodoroTimer.Api.csproj
```

Ou utiliser la tâche VS Code intégrée :
- Appuyer sur `Ctrl+Shift+B` pour lancer le build par défaut

**En mode release :**
```bash
dotnet build -c Release PomodoroTimer.Api/PomodoroTimer.Api.csproj
```

### Frontend

**Build de développement :**
```bash
cd frontend
npm run build
```

**Build optimisé (production) :**
```bash
npm run build -- --mode production
```

---

## ▶️ Guide d'exécution du projet

### Lancer le backend

```bash
cd backend/PomodoroTimer.Api
dotnet run
```

L'API sera accessible à : `https://localhost:5001` ou `https://localhost:5000`

### Lancer le frontend

**Mode développement avec hot reload :**
```bash
cd frontend
npm run dev
```

L'application sera accessible à : `http://localhost:5173`

### Déploiement Azure

Pour déployer sur Azure, utilisez les scripts fournis :

```bash
# Backend
cd infrastructure/deployment/azure-cli
./01_deploy-backend-with-azure-cli.ps1

# Frontend
./02_deploy-frontend-with-azure-cli.ps1

# Configuration CORS
./03_enable-cors-on-backend-with-azure-cli.ps1
```

---

## ✅ Lancement des tests

### Tests backend

```bash
cd backend
# Exécuter tous les tests
dotnet test

# Ou tests spécifiques
dotnet test --filter "ClassName"
```

### Tests frontend

```bash
cd frontend

# Lancer les tests unitaires
npm run test

# Lancer les tests E2E
npm run test:e2e

# Générer un rapport de couverture
npm run test:coverage
```

---

## 📦 Structure du projet

```
workshop-ghc/
├── backend/                          # API ASP.NET Core
│   ├── PomodoroTimer.Api/
│   │   ├── Controllers/              # Points d'entrée HTTP
│   │   │   ├── AuthController.cs
│   │   │   ├── SessionsController.cs
│   │   │   ├── SettingsController.cs
│   │   │   └── StatsController.cs
│   │   ├── Services/                 # Logique métier
│   │   │   ├── AuthService.cs
│   │   │   ├── SessionService.cs
│   │   │   └── SettingsService.cs
│   │   ├── Models/                   # Entités de domaine
│   │   │   ├── User.cs
│   │   │   ├── PomodoroSession.cs
│   │   │   └── UserSettings.cs
│   │   ├── Data/                     # Contexte Entity Framework
│   │   │   └── PomodoroDbContext.cs
│   │   ├── BusinessObjects/          # DTOs et objets métier
│   │   ├── Requests/                 # Objets de requête
│   │   ├── Responses/                # Objets de réponse
│   │   └── Program.cs                # Configuration Startup
│   └── PomodoroTimer.sln
│
├── frontend/                         # Application Vue 3
│   ├── src/
│   │   ├── views/                    # Pages principales
│   │   │   ├── HomeView.vue          # Timer
│   │   │   ├── ProfileView.vue       # Statistiques
│   │   │   └── SettingsView.vue      # Configuration
│   │   ├── components/               # Composants réutilisables
│   │   ├── stores/                   # Pinia state management
│   │   ├── api/                      # Services API
│   │   ├── router/                   # Configuration routing
│   │   ├── types/                    # Types TypeScript
│   │   └── App.vue                   # Composant root
│   ├── public/                       # Ressources statiques
│   └── vite.config.ts                # Configuration Vite
│
├── infrastructure/                   # Déploiement et infra
│   └── deployment/
│       ├── azure-cli/                # Scripts Azure
│       └── local/                    # Documentation locale
│
├── sql/                              # Scripts de base de données
│   └── tables/
│       ├── 01_users.sql
│       ├── 02_user_settings.sql
│       └── 03_pomodoro_sessions.sql
│
└── README.md                         # Ce fichier
```

### Légende des répertoires

| Dossier | Description |
|---------|-------------|
| `backend` | API REST ASP.NET Core + logique métier |
| `frontend` | Interface utilisateur Vue 3 |
| `infrastructure` | Scripts de déploiement et configuration |
| `sql` | Schéma et scripts de création de base de données |

---

## 🤝 Contribution et bonnes pratiques

### Workflow de contribution

1. **Créer une branche** pour votre feature/bugfix :
   ```bash
   git checkout -b feature/nom-feature
   git checkout -b bugfix/description-bug
   ```

2. **Commits clairs et atomiques** :
   ```bash
   git commit -m "feat: description courte"
   git commit -m "fix: description du bug"
   ```

3. **Push et Pull Request** :
   ```bash
   git push origin feature/nom-feature
   ```

### Conventions de code

**Backend (C#) :**
- Utiliser PascalCase pour les classes et méthodes publiques
- Utiliser camelCase pour les variables locales
- Respecter les principes SOLID
- Commenter les logiques complexes

**Frontend (Vue/TypeScript) :**
- Utiliser camelCase pour les variables et fonctions
- Utiliser PascalCase pour les noms de composants
- Prettier pour la mise en forme : `npm run format`
- ESLint pour les vérifications : `npm run lint`

### Avant de faire un commit

```bash
# Backend
cd backend && dotnet build && dotnet test

# Frontend
cd frontend && npm run lint && npm run test
```

### Structure des branches

- `main` - Production (protégée)
- `develop` - Intégration
- `feature/*` - Nouvelles fonctionnalités
- `bugfix/*` - Corrections de bugs
- `hotfix/*` - Corrections urgentes

---

## 📄 Licence

Ce projet est fourni à titre éducatif pour l'atelier **GitHub Copilot Workshop**. 

Tous droits réservés © 2026.

---

## 📞 Support et Contact

Pour toute question sur le projet :
- Consulter la documentation dans `infrastructure/deployment/local/`
- Vérifier les logs des services en cours d'exécution
- Vérifier la configuration des variables d'environnement

---

**Dernière mise à jour** : Mars 2026
