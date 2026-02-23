import apiClient from './client'
import type { UserSettings } from '@/types'

export interface SaveSettingsRequest {
  publicUsername: string | null
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

export const settingsApi = {
  async saveSettings(settings: SaveSettingsRequest): Promise<void> {
    await apiClient.post('/api/settings', settings)
  },

  async getSettings(): Promise<UserSettings> {
    const response = await apiClient.get<UserSettings>('/api/settings')
    return response.data
  }
}
