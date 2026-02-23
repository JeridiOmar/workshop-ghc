export type SessionType = 'pomodoro' | 'short_break' | 'long_break'

export interface UserSettings {
  publicUsername: string | null
  anonymousNumber: number | null
  pomodoroDuration: number
  shortBreakDuration: number
  longBreakDuration: number
  notificationsEnabled: boolean
  notificationSound: string
  notificationVolume: number
  themeFocus: string
  themeShortBreak: string
  themeLongBreak: string
  autoStartBreaks: boolean
  autoStartPomodoro: boolean
  longBreakInterval: number
}

export interface PomodoroSession {
  id?: number
  userId?: string
  sessionType: SessionType
  durationSeconds: number
  isCompleted: boolean
  completedAt: string
}

export interface User {
  id: string
  username: string
  bearerToken: string
  createdAt: string
}
