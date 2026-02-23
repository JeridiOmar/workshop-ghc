<script setup lang="ts">
import { ref } from 'vue'
import { useSettingsStore } from '@/stores/settings'
import { useAuthStore } from '@/stores/auth'
import { settingsApi } from '@/api/settings'
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Button } from '@/components/ui/button'
import { Volume2, Bell, BellOff } from 'lucide-vue-next'

const settingsStore = useSettingsStore()
const authStore = useAuthStore()
const settings = ref(settingsStore.settings)
const showSaveMessage = ref(false)
const isSaving = ref(false)
const saveError = ref('')

const soundOptions = [
  { value: 'bell_1', label: 'Bell 1' },
  { value: 'bell_2', label: 'Bell 2' },
  { value: 'bell_3', label: 'Bell 3' },
  { value: 'bell_4', label: 'Bell 4' }
]

async function saveSettings() {
  saveError.value = ''
  isSaving.value = true
  
  try {
    // Always update local settings
    settingsStore.updateSettings(settings.value)
    
    // If authenticated, also save to API
    if (authStore.isAuthenticated) {
      await settingsApi.saveSettings({
        publicUsername: settings.value.publicUsername,
        pomodoroDuration: settings.value.pomodoroDuration,
        shortBreakDuration: settings.value.shortBreakDuration,
        longBreakDuration: settings.value.longBreakDuration,
        notificationsEnabled: settings.value.notificationsEnabled,
        notificationSound: settings.value.notificationSound,
        notificationVolume: settings.value.notificationVolume,
        themeFocus: settings.value.themeFocus,
        themeShortBreak: settings.value.themeShortBreak,
        themeLongBreak: settings.value.themeLongBreak,
        autoStartBreaks: settings.value.autoStartBreaks,
        autoStartPomodoro: settings.value.autoStartPomodoro,
        longBreakInterval: settings.value.longBreakInterval
      })
    }
    
    showSaveMessage.value = true
    setTimeout(() => {
      showSaveMessage.value = false
    }, 3000)
  } catch (error: any) {
    saveError.value = error.response?.data?.message || 'Failed to save settings'
  } finally {
    isSaving.value = false
  }
}

function previewSound() {
  try {
    const audio = new Audio(`/sounds/${settings.value.notificationSound}.mp3`)
    audio.volume = settings.value.notificationVolume / 100
    audio.play().catch((error) => {
      console.error('Error playing sound:', error)
    })
  } catch (error) {
    console.error('Error creating audio:', error)
  }
}

function resetToDefaults() {
  if (confirm('Are you sure you want to reset all settings to defaults?')) {
    settingsStore.resetToDefaults()
    settings.value = settingsStore.settings
    saveSettings()
  }
}

</script>

<template>
  <div class="max-w-4xl mx-auto">
    <h1 class="text-4xl font-bold text-white mb-8">Settings</h1>

    <div class="space-y-6">
      <!-- Profile Settings -->
      <Card class="bg-white/10 backdrop-blur-sm border-white/20">
        <CardHeader>
          <CardTitle class="text-white">Profile</CardTitle>
        </CardHeader>
        <CardContent class="space-y-4">
          <div>
            <label class="block text-white/80 text-sm font-medium mb-2">
              Public Username
            </label>
            <Input
              :model-value="settings.publicUsername ?? ''"
              @update:model-value="(val: string | number) => settings.publicUsername = val?.toString() || null"
              placeholder="Enter your display name (optional)"
              class="bg-white/10 border-white/20 text-white placeholder:text-white/40"
              maxlength="50"
            />
          </div>
        </CardContent>
      </Card>

      <!-- Timer Duration Settings -->
      <Card class="bg-white/10 backdrop-blur-sm border-white/20">
        <CardHeader>
          <CardTitle class="text-white">Timer Durations</CardTitle>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <label class="block text-white/80 text-sm font-medium mb-2">
                Pomodoro (minutes)
              </label>
              <Input
                v-model.number="settings.pomodoroDuration"
                type="number"
                min="1"
                max="60"
                class="bg-white/10 border-white/20 text-white"
              />
            </div>
            <div>
              <label class="block text-white/80 text-sm font-medium mb-2">
                Short Break (minutes)
              </label>
              <Input
                v-model.number="settings.shortBreakDuration"
                type="number"
                min="1"
                max="30"
                class="bg-white/10 border-white/20 text-white"
              />
            </div>
            <div>
              <label class="block text-white/80 text-sm font-medium mb-2">
                Long Break (minutes)
              </label>
              <Input
                v-model.number="settings.longBreakDuration"
                type="number"
                min="1"
                max="60"
                class="bg-white/10 border-white/20 text-white"
              />
            </div>
          </div>
        </CardContent>
      </Card>

      <!-- Notification Settings -->
      <Card class="bg-white/10 backdrop-blur-sm border-white/20">
        <CardHeader>
          <CardTitle class="text-white">Notifications</CardTitle>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-2">
              <Bell v-if="settings.notificationsEnabled" class="h-5 w-5 text-white/80" />
              <BellOff v-else class="h-5 w-5 text-white/80" />
              <span class="text-white/80">Enable Notifications</span>
            </div>
            <button
              @click="settings.notificationsEnabled = !settings.notificationsEnabled"
              :class="[
                'relative inline-flex h-6 w-11 items-center rounded-full transition-colors',
                settings.notificationsEnabled ? 'bg-white/80' : 'bg-white/20'
              ]"
            >
              <span
                :class="[
                  'inline-block h-4 w-4 transform rounded-full bg-white transition-transform',
                  settings.notificationsEnabled ? 'translate-x-6' : 'translate-x-1'
                ]"
              />
            </button>
          </div>

          <div>
            <label class="block text-white/80 text-sm font-medium mb-2">
              Notification Sound
            </label>
            <div class="flex gap-2">
              <select
                v-model="settings.notificationSound"
                class="flex-1 h-10 rounded-md border border-white/20 bg-white/10 px-3 py-2 text-sm text-white [&>option]:bg-gray-800 [&>option]:text-white"
              >
                <option v-for="sound in soundOptions" :key="sound.value" :value="sound.value">
                  {{ sound.label }}
                </option>
              </select>
              <Button
                variant="outline"
                size="icon"
                class="border-white/40 text-white hover:bg-white/20"
                @click="previewSound"
              >
                <Volume2 :size="16" />
              </Button>
            </div>
          </div>

          <div>
            <label class="block text-white/80 text-sm font-medium mb-2">
              Volume: {{ settings.notificationVolume }}%
            </label>
            <input
              v-model.number="settings.notificationVolume"
              type="range"
              min="0"
              max="100"
              class="w-full h-2 bg-white/20 rounded-lg appearance-none cursor-pointer"
            />
          </div>
        </CardContent>
      </Card>

      <!-- Theme Settings -->
      <Card class="bg-white/10 backdrop-blur-sm border-white/20">
        <CardHeader>
          <CardTitle class="text-white">Themes</CardTitle>
        </CardHeader>
        <CardContent class="space-y-4">
          <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <label class="block text-white/80 text-sm font-medium mb-2">
                Focus Mode Color
              </label>
              <div class="flex gap-2">
                <input
                  v-model="settings.themeFocus"
                  type="color"
                  class="h-10 w-full rounded-md border border-white/20 bg-white/10 cursor-pointer"
                />
                <Input
                  v-model="settings.themeFocus"
                  class="flex-1 bg-white/10 border-white/20 text-white"
                />
              </div>
            </div>
            <div>
              <label class="block text-white/80 text-sm font-medium mb-2">
                Short Break Color
              </label>
              <div class="flex gap-2">
                <input
                  v-model="settings.themeShortBreak"
                  type="color"
                  class="h-10 w-full rounded-md border border-white/20 bg-white/10 cursor-pointer"
                />
                <Input
                  v-model="settings.themeShortBreak"
                  class="flex-1 bg-white/10 border-white/20 text-white"
                />
              </div>
            </div>
            <div>
              <label class="block text-white/80 text-sm font-medium mb-2">
                Long Break Color
              </label>
              <div class="flex gap-2">
                <input
                  v-model="settings.themeLongBreak"
                  type="color"
                  class="h-10 w-full rounded-md border border-white/20 bg-white/10 cursor-pointer"
                />
                <Input
                  v-model="settings.themeLongBreak"
                  class="flex-1 bg-white/10 border-white/20 text-white"
                />
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      <!-- Actions -->
      <div class="flex gap-4">
        <Button
          @click="saveSettings"
          size="lg"
          class="bg-white text-gray-900 hover:bg-white/90"
        >
          Save Settings
        </Button>
        <Button
          @click="resetToDefaults"
          size="lg"
          variant="outline"
          class="border-white/40 text-white hover:bg-white/20"
        >
          Reset to Defaults
        </Button>
      </div>

      <!-- Save Message -->
      <div
        v-if="showSaveMessage"
        class="fixed bottom-4 right-4 bg-white text-gray-900 px-6 py-3 rounded-lg shadow-lg"
      >
        ✓ Settings saved successfully!
      </div>
    </div>
  </div>
</template>
