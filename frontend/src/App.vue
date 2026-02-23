<script setup lang="ts">
import { computed } from 'vue'
import { RouterView, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useTimerStore } from '@/stores/timer'
import { LogOut } from 'lucide-vue-next'

const authStore = useAuthStore()
const timerStore = useTimerStore()
const router = useRouter()

const bgStyle = computed(() => ({
  backgroundColor: timerStore.currentTheme,
  transition: 'background-color 0.3s ease'
}))

function handleLogout() {
  authStore.logout()
  router.push('/profile')
}
</script>

<template>
  <div class="min-h-screen" :style="bgStyle">
    <!-- Navigation -->
    <nav class="bg-white/10 backdrop-blur-sm border-b border-white/20">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16">
          <div class="flex items-center">
            <router-link to="/" class="text-2xl font-bold text-white !bg-transparent" active-class="">
              Pomodoro
            </router-link>
            
            <!-- Desktop Menu -->
            <div class="hidden md:ml-10 md:flex md:space-x-8">
              <router-link 
                to="/" 
                class="text-white/80 hover:text-white px-3 py-2 rounded-md text-sm font-medium transition"
                active-class="text-white bg-white/20"
              >
                Timer
              </router-link>
              <router-link 
                to="/settings" 
                class="text-white/80 hover:text-white px-3 py-2 rounded-md text-sm font-medium transition"
                active-class="text-white bg-white/20"
              >
                Settings
              </router-link>
            </div>
          </div>
          
          <!-- Desktop User Menu -->
          <div class="hidden md:flex md:items-center md:space-x-4">
            <template v-if="authStore.isAuthenticated">
              <span class="text-white text-sm font-medium">
                {{ authStore.user?.username }}
              </span>
              <button
                @click="handleLogout"
                class="text-white/80 hover:text-white px-3 py-2 rounded-md text-sm font-medium transition flex items-center gap-2"
                title="Logout"
              >
                <LogOut class="h-4 w-4" />
                <span>Logout</span>
              </button>
            </template>
            <template v-else>
              <router-link 
                to="/profile"
                class="text-white/80 hover:text-white px-3 py-2 rounded-md text-sm font-medium transition"
              >
                Profile
              </router-link>
            </template>
          </div>
        </div>
      </div>
    </nav>
    
    <!-- Main Content -->
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
.router-link-active {
  background-color: rgba(255, 255, 255, 0.2);
  color: white;
}
</style>
