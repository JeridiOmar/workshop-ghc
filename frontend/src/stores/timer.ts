import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { SessionType } from '@/types'
import { useSettingsStore } from './settings'
import { useAuthStore } from './auth'
import { sessionsApi } from '@/api/sessions'

export const useTimerStore = defineStore('timer', () => {
  const settingsStore = useSettingsStore()
  const authStore = useAuthStore()
  
  const currentMode = ref<SessionType>('pomodoro')
  const timeRemaining = ref(0) // in seconds
  const isRunning = ref(false)
  const isPaused = ref(false)
  const sessionCount = ref(0)
  const completedPomodoros = ref(0)
  
  let intervalId: number | null = null
  let sessionStartTime: number = 0
  
  const currentDuration = computed(() => {
    switch (currentMode.value) {
      case 'pomodoro':
        return settingsStore.settings.pomodoroDuration * 60
      case 'short_break':
        return settingsStore.settings.shortBreakDuration * 60
      case 'long_break':
        return settingsStore.settings.longBreakDuration * 60
      default:
        return 0
    }
  })
  
  const currentTheme = computed(() => {
    switch (currentMode.value) {
      case 'pomodoro':
        return settingsStore.settings.themeFocus
      case 'short_break':
        return settingsStore.settings.themeShortBreak
      case 'long_break':
        return settingsStore.settings.themeLongBreak
      default:
        return settingsStore.settings.themeFocus
    }
  })
  
  const progress = computed(() => {
    return ((currentDuration.value - timeRemaining.value) / currentDuration.value) * 100
  })
  
  function initializeTimer() {
    timeRemaining.value = currentDuration.value
    sessionStartTime = Date.now()
  }
  
  function start() {
    if (!isRunning.value) {
      isRunning.value = true
      isPaused.value = false
      sessionStartTime = Date.now() - ((currentDuration.value - timeRemaining.value) * 1000)
      
      intervalId = window.setInterval(() => {
        if (timeRemaining.value > 0) {
          timeRemaining.value--
          
          // Update document title
          const mins = Math.floor(timeRemaining.value / 60)
          const secs = timeRemaining.value % 60
          document.title = `${mins}:${secs.toString().padStart(2, '0')} - Pomodoro Timer`
        } else {
          complete()
        }
      }, 1000)
    }
  }
  
  function pause() {
    if (isRunning.value && intervalId !== null) {
      clearInterval(intervalId)
      intervalId = null
      isRunning.value = false
      isPaused.value = true
    }
  }
  
  function reset() {
    if (intervalId !== null) {
      clearInterval(intervalId)
      intervalId = null
    }
    isRunning.value = false
    isPaused.value = false
    timeRemaining.value = currentDuration.value
    sessionStartTime = 0
    document.title = 'Pomodoro Timer'
  }
  
  async function trackSession(durationSeconds: number, isCompleted: boolean) {
    // Only track if user is authenticated
    if (!authStore.isAuthenticated) {
      return
    }
    
    try {
      await sessionsApi.createSession({
        type: currentMode.value,
        durationSeconds,
        isCompleted,
        completedAt: new Date().toISOString()
      })
    } catch (error) {
      console.error('Failed to track session:', error)
    }
  }
  
  function complete() {
    if (intervalId !== null) {
      clearInterval(intervalId)
      intervalId = null
    }
    
    const elapsedSeconds = Math.floor((Date.now() - sessionStartTime) / 1000)
    
    // Track completed session
    if (elapsedSeconds > 0) {
      trackSession(elapsedSeconds, true)
    }
    
    if (currentMode.value === 'pomodoro') {
      completedPomodoros.value++
    }
    
    isRunning.value = false
    isPaused.value = false
    
    // Show notification
    if (settingsStore.settings.notificationsEnabled && 'Notification' in window) {
      if (Notification.permission === 'granted') {
        new Notification('Pomodoro Timer', {
          body: currentMode.value === 'pomodoro' 
            ? 'Time for a break!' 
            : 'Time to focus!',
          icon: '/pomodoro-icon.png'
        })
      }
    }
    
    // Play sound
    playNotificationSound()
    
    // Auto-switch to next mode
    nextMode()
  }
  
  function skip() {
    const wasRunning = isRunning.value
    
    // Track incomplete session if there was progress
    if (wasRunning && sessionStartTime > 0) {
      const elapsedSeconds = Math.floor((Date.now() - sessionStartTime) / 1000)
      if (elapsedSeconds > 0) {
        trackSession(elapsedSeconds, false)
      }
    }
    
    reset()
    nextMode()
  }
  
  function nextMode() {
    if (currentMode.value === 'pomodoro') {
      // Decide between short and long break
      const shouldBeLongBreak = completedPomodoros.value > 0 && 
        completedPomodoros.value % settingsStore.settings.longBreakInterval === 0
      
      currentMode.value = shouldBeLongBreak ? 'long_break' : 'short_break'
    } else {
      currentMode.value = 'pomodoro'
      sessionCount.value++
    }
    
    initializeTimer()
    
    // Auto-start if enabled
    const shouldAutoStart = currentMode.value === 'pomodoro' 
      ? settingsStore.settings.autoStartPomodoro
      : settingsStore.settings.autoStartBreaks
    
    if (shouldAutoStart) {
      start()
    }
  }
  
  function switchMode(mode: SessionType) {
    reset()
    currentMode.value = mode
    initializeTimer()
  }
  
  function playNotificationSound() {
    const audio = new Audio(`/sounds/${settingsStore.settings.notificationSound}.mp3`)
    audio.volume = settingsStore.settings.notificationVolume / 100
    audio.play().catch(console.error)
  }
  
  // Initialize on store creation
  initializeTimer()
  
  return {
    currentMode,
    timeRemaining,
    isRunning,
    isPaused,
    sessionCount,
    completedPomodoros,
    currentDuration,
    currentTheme,
    progress,
    start,
    pause,
    reset,
    skip,
    switchMode,
    playNotificationSound
  }
})
