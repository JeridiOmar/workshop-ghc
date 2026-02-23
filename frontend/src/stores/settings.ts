import { defineStore } from 'pinia'
import { ref, computed, watch } from 'vue'
import type { UserSettings } from '@/types'

const DEFAULT_SETTINGS: UserSettings = {
  publicUsername: null,
  anonymousNumber: null,
  pomodoroDuration: 25,
  shortBreakDuration: 5,
  longBreakDuration: 15,
  notificationsEnabled: true,
  notificationSound: 'bell_1',
  notificationVolume: 80,
  themeFocus: '#BA4949',
  themeShortBreak: '#38858A',
  themeLongBreak: '#397097',
  autoStartBreaks: false,
  autoStartPomodoro: false,
  longBreakInterval: 4
}

export const useSettingsStore = defineStore('settings', () => {
  const settings = ref<UserSettings>({ ...DEFAULT_SETTINGS })
  
  // Load from localStorage
  const loadSettings = () => {
    const stored = localStorage.getItem('settings')
    if (stored) {
      try {
        const parsed = JSON.parse(stored)
        // Migrate old sound values to new ones
        if (parsed.notificationSound === 'bell' || !['bell_1', 'bell_2', 'bell_3', 'bell_4'].includes(parsed.notificationSound)) {
          parsed.notificationSound = 'bell_1'
        }
        settings.value = { ...DEFAULT_SETTINGS, ...parsed }
      } catch {
        settings.value = { ...DEFAULT_SETTINGS }
      }
    }
  }
  
  // Save to localStorage
  const saveSettings = () => {
    localStorage.setItem('settings', JSON.stringify(settings.value))
  }
  
  // Watch for changes and auto-save
  watch(settings, saveSettings, { deep: true })
  
  // Initialize
  loadSettings()
  
  // Generate anonymous number if not set
  if (!settings.value.anonymousNumber) {
    settings.value.anonymousNumber = Math.floor(Math.random() * 1000) + 1
    saveSettings()
  }
  
  const displayName = computed(() => {
    if (settings.value.publicUsername && settings.value.publicUsername.trim() !== '') {
      return settings.value.publicUsername
    }

    const num = settings.value.anonymousNumber
    if (num === null || num === undefined) {
      return 'Anonymous'
    }

    return `Anonymous ${num}`
  })
  
  function updateSettings(partial: Partial<UserSettings>) {
    settings.value = { ...settings.value, ...partial }
  }
  
  function resetToDefaults() {
    settings.value = { ...DEFAULT_SETTINGS }
  }
  
  return {
    settings,
    displayName,
    updateSettings,
    resetToDefaults,
    saveSettings,
    loadSettings
  }
})
