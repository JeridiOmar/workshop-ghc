# Pomodoro Timer - Frontend

A modern, customizable Pomodoro timer application built with Vue.js 3, TypeScript, and Tailwind CSS.

## Features

- ⏱️ **Pomodoro Timer** - 25-minute focus sessions with customizable durations
- ☕ **Break Management** - Short (5min) and long (15min) breaks
- 🎨 **Customizable Themes** - Different colors for each timer mode
- 🔔 **Notifications** - Browser notifications and custom sounds
- 📊 **Progress Tracking** - View your daily, weekly, and all-time statistics
- 🏆 **Global Rankings** - Compete with other users on the leaderboard
- 🔐 **User Authentication** - Sign up and sync your settings across devices
- 💾 **Local Storage** - Settings persist for guest users
- 📱 **Responsive Design** - Works on desktop, tablet, and mobile

## Tech Stack

- **Vue 3** - Composition API with TypeScript
- **Vite** - Fast build tool and dev server
- **Pinia** - State management
- **Vue Router** - Client-side routing
- **Tailwind CSS** - Utility-first CSS framework
- **shadcn-vue** - Beautiful UI components based on Radix Vue
- **Lucide Icons** - Clean, consistent icons
- **VueUse** - Composition utilities

## Project Structure

```
frontend/
├── public/              # Static assets
│   └── sounds/         # Notification sound files
├── src/
│   ├── api/            # API client and services
│   ├── assets/         # Images, fonts, etc.
│   ├── components/     # Vue components
│   │   └── ui/        # shadcn-vue UI components
│   ├── composables/    # Reusable composition functions
│   ├── lib/           # Utilities and helpers
│   ├── router/        # Vue Router configuration
│   ├── stores/        # Pinia stores
│   ├── types/         # TypeScript type definitions
│   ├── views/         # Page components
│   ├── App.vue        # Root component
│   ├── main.ts        # Application entry point
│   └── style.css      # Global styles (Tailwind)
├── index.html         # HTML template
├── package.json       # Dependencies
├── tailwind.config.js # Tailwind configuration
├── tsconfig.json      # TypeScript configuration
└── vite.config.ts     # Vite configuration
```

## Getting Started

### Prerequisites

- Node.js 18+ and npm

### Installation

```bash
# Install dependencies
npm install

# Start development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview
```

The app will be available at `http://localhost:5000/`

## Usage

### Guest Mode

- Start using the timer immediately without signing up
- Settings are saved locally in your browser
- Access to timer and settings pages

### Authenticated Mode

- Create profile with username and 6-digit PIN
- Settings sync across devices
- Access to reports and global rankings
- Set a public display name for the leaderboard (different from login username)

### Timer Controls

- **START/PAUSE** - Begin or pause the timer
- **SKIP** - Move to the next session
- **RESET** - Reset current timer to initial duration

### Settings

1. **Profile** - Set your public username for rankings
2. **Timer Durations** - Customize Pomodoro, short break, and long break durations
3. **Notifications** - Enable/disable notifications, choose sound, adjust volume
4. **Themes** - Customize background colors for each mode
5. **Auto-Start** - Automatically start breaks or pomodoros

### Reports

- **Summary Tab** - View your daily, weekly, and all-time statistics
- **Ranking Tab** - See how you rank against other users globally

## Configuration

### Environment Variables

Create a `.env` file in the root directory:

```env
VITE_API_BASE_URL=http://localhost:5173/api
```

### Notification Sounds

Add MP3 files to `public/sounds/`:
- bell.mp3
- chime.mp3
- digital.mp3
- kitchen.mp3
- soft.mp3
- wood.mp3

## API Integration

The frontend is designed to integrate with an ASP.NET Core Web API (.NET 9) backend. API endpoints:

**Authentication:**
- `POST /api/auth/register` - Create new profile (username + 6-digit PIN)
- `POST /api/auth/login` - Load existing profile

**User Settings:**
- `GET /api/settings` - Fetch user settings (requires auth)
- `POST /api/settings` - Save user settings (requires auth)

**Sessions:**
- `POST /api/sessions` - Record completed/incomplete session (requires auth) when user click on forward button
- `GET /api/sessions` - Get session history (requires auth)

**Statistics:**
- `GET /api/stats/summary` - Get user statistics (requires auth)
- `GET /api/stats/ranking` - Get global rankings

## State Management

### Stores

- **authStore** - User authentication state
- **settingsStore** - User preferences and configuration
- **timerStore** - Timer state and controls

### Local Storage Keys

- `user` - Authenticated user information
- `token` - Authentication token
- `settings` - User settings (for guest users)

## Browser Support

- Chrome (latest 2 versions)
- Firefox (latest 2 versions)
- Safari (latest 2 versions)
- Edge (latest 2 versions)

## Accessibility

- ARIA labels for screen readers
- Keyboard navigation support
- High contrast mode compatible
- Focus indicators

## License

MIT License - See LICENSE file for details

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## Support

For issues and feature requests, please use the GitHub issues page.
