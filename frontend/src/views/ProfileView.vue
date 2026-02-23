<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Button } from '@/components/ui/button'
import { UserCircle, LogIn } from 'lucide-vue-next'

const router = useRouter()
const authStore = useAuthStore()

const mode = ref<'select' | 'create' | 'load'>('select')
const username = ref('')
const pin = ref('')

function showCreateForm() {
  mode.value = 'create'
  authStore.clearError()
  resetForm()
}

function showLoadForm() {
  mode.value = 'load'
  authStore.clearError()
  resetForm()
}

function backToSelect() {
  mode.value = 'select'
  authStore.clearError()
  resetForm()
}

function resetForm() {
  username.value = ''
  pin.value = ''
}

function validatePin(value: string): boolean {
  return /^\d{6}$/.test(value)
}

async function handleCreate() {
  authStore.clearError()
  
  // Validate username
  if (!username.value || username.value.trim().length < 3) {
    authStore.error = 'Username must be at least 3 characters'
    return
  }
  
  if (username.value.trim().length > 50) {
    authStore.error = 'Username must be less than 50 characters'
    return
  }
  
  // Validate PIN
  if (!validatePin(pin.value)) {
    authStore.error = 'PIN must be exactly 6 digits'
    return
  }
  
  const success = await authStore.register({
    username: username.value.trim(),
    pin: pin.value
  })
  
  if (success) {
    router.push('/')
  }
}

async function handleLoad() {
  authStore.clearError()
  
  // Validate username
  if (!username.value || username.value.trim().length < 3) {
    authStore.error = 'Please enter your username'
    return
  }
  
  // Validate PIN
  if (!validatePin(pin.value)) {
    authStore.error = 'PIN must be exactly 6 digits'
    return
  }
  
  const success = await authStore.login({
    username: username.value.trim(),
    pin: pin.value
  })
  
  if (success) {
    router.push('/')
  }
}

function handlePinInput(event: Event) {
  const target = event.target as HTMLInputElement
  target.value = target.value.replace(/\D/g, '').slice(0, 6)
  pin.value = target.value
}
</script>

<template>
  <div class="min-h-[calc(100vh-12rem)] flex items-center justify-center">
    <!-- Selection Mode -->
    <Card v-if="mode === 'select'" class="w-full max-w-md bg-white/10 backdrop-blur-sm border-white/20">
      <CardHeader>
        <CardTitle class="text-white text-3xl text-center">Profile</CardTitle>
      </CardHeader>
      <CardContent class="space-y-4">
        <p class="text-white/80 text-center mb-6">
          Create a new profile or load an existing one to save your progress and access rankings.
        </p>
        
        <Button
          size="lg"
          class="w-full bg-white text-gray-900 hover:bg-white/90"
          @click="showCreateForm"
        >
          <UserCircle class="mr-2 h-5 w-5" />
          Create New Profile
        </Button>
        
        <Button
          size="lg"
          variant="outline"
          class="w-full border-white/40 text-white hover:bg-white/20"
          @click="showLoadForm"
        >
          <LogIn class="mr-2 h-5 w-5" />
          Load Existing Profile
        </Button>
        
        <div class="text-center pt-4">
          <router-link to="/" class="text-white/60 hover:text-white text-sm">
            Continue as guest
          </router-link>
        </div>
      </CardContent>
    </Card>

    <!-- Create Profile Mode -->
    <Card v-else-if="mode === 'create'" class="w-full max-w-md bg-white/10 backdrop-blur-sm border-white/20">
      <CardHeader>
        <CardTitle class="text-white text-3xl text-center">Create Profile</CardTitle>
      </CardHeader>
      <CardContent>
        <form @submit.prevent="handleCreate" class="space-y-4">
          <div v-if="authStore.error" class="bg-red-500/20 border border-red-500/50 text-white px-4 py-3 rounded">
            {{ authStore.error }}
          </div>

          <div>
            <label class="block text-white/80 text-sm font-medium mb-2">
              Username
            </label>
            <Input
              v-model="username"
              type="text"
              placeholder="Enter your username (spaces allowed)"
              required
              minlength="3"
              maxlength="50"
              class="bg-white/10 border-white/20 text-white placeholder:text-white/40"
              :disabled="authStore.isLoading"
            />
            <p class="text-white/60 text-xs mt-1">
              3-50 characters, spaces allowed
            </p>
          </div>

          <div>
            <label class="block text-white/80 text-sm font-medium mb-2">
              6-Digit PIN
            </label>
            <Input
              v-model="pin"
              type="password"
              placeholder="Enter 6-digit PIN"
              required
              pattern="\d{6}"
              maxlength="6"
              inputmode="numeric"
              class="bg-white/10 border-white/20 text-white placeholder:text-white/40"
              :disabled="authStore.isLoading"
              @input="handlePinInput"
            />
            <p class="text-white/60 text-xs mt-1">
              Must be exactly 6 digits (0-9)
            </p>
          </div>

          <div class="flex gap-3 pt-2">
            <Button
              type="button"
              variant="outline"
              class="flex-1 border-white/40 text-white hover:bg-white/20"
              @click="backToSelect"
              :disabled="authStore.isLoading"
            >
              Back
            </Button>
            <Button
              type="submit"
              class="flex-1 bg-white text-gray-900 hover:bg-white/90"
              :disabled="authStore.isLoading"
            >
              {{ authStore.isLoading ? 'Creating...' : 'Create Profile' }}
            </Button>
          </div>
        </form>
      </CardContent>
    </Card>

    <!-- Load Profile Mode -->
    <Card v-else-if="mode === 'load'" class="w-full max-w-md bg-white/10 backdrop-blur-sm border-white/20">
      <CardHeader>
        <CardTitle class="text-white text-3xl text-center">Load Profile</CardTitle>
      </CardHeader>
      <CardContent>
        <form @submit.prevent="handleLoad" class="space-y-4">
          <div v-if="authStore.error" class="bg-red-500/20 border border-red-500/50 text-white px-4 py-3 rounded">
            {{ authStore.error }}
          </div>

          <div>
            <label class="block text-white/80 text-sm font-medium mb-2">
              Username
            </label>
            <Input
              v-model="username"
              type="text"
              placeholder="Enter your username"
              required
              class="bg-white/10 border-white/20 text-white placeholder:text-white/40"
              :disabled="authStore.isLoading"
            />
          </div>

          <div>
            <label class="block text-white/80 text-sm font-medium mb-2">
              6-Digit PIN
            </label>
            <Input
              v-model="pin"
              type="password"
              placeholder="Enter your 6-digit PIN"
              required
              pattern="\d{6}"
              maxlength="6"
              inputmode="numeric"
              class="bg-white/10 border-white/20 text-white placeholder:text-white/40"
              :disabled="authStore.isLoading"
              @input="handlePinInput"
            />
          </div>

          <div class="flex gap-3 pt-2">
            <Button
              type="button"
              variant="outline"
              class="flex-1 border-white/40 text-white hover:bg-white/20"
              @click="backToSelect"
              :disabled="authStore.isLoading"
            >
              Back
            </Button>
            <Button
              type="submit"
              class="flex-1 bg-white text-gray-900 hover:bg-white/90"
              :disabled="authStore.isLoading"
            >
              {{ authStore.isLoading ? 'Loading...' : 'Load Profile' }}
            </Button>
          </div>
        </form>
      </CardContent>
    </Card>
  </div>
</template>
