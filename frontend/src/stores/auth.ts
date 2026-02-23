import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { User } from '@/types'
import { authApi, type RegisterRequest, type LoginRequest } from '@/api/auth'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const isAuthenticated = computed(() => user.value !== null)
  const error = ref<string>('')
  const isLoading = ref(false)
  
  // Load from localStorage on init
  const storedUser = localStorage.getItem('user')
  const storedToken = localStorage.getItem('token')
  if (storedUser && storedToken) {
    try {
      user.value = JSON.parse(storedUser)
    } catch {
      localStorage.removeItem('user')
      localStorage.removeItem('token')
    }
  }

  function setUser(newUser: User | null) {
    user.value = newUser
    if (newUser) {
      localStorage.setItem('user', JSON.stringify(newUser))
      localStorage.setItem('token', newUser.bearerToken)
    } else {
      localStorage.removeItem('user')
      localStorage.removeItem('token')
    }
  }

  async function register(data: RegisterRequest): Promise<boolean> {
    error.value = ''
    isLoading.value = true
    
    try {
      const response = await authApi.register(data)
      setUser({
        id: response.userId,
        username: response.username,
        bearerToken: response.bearerToken,
        createdAt: new Date().toISOString()
      })
      return true
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Registration failed. Please try again.'
      return false
    } finally {
      isLoading.value = false
    }
  }

  async function login(data: LoginRequest): Promise<boolean> {
    error.value = ''
    isLoading.value = true
    
    try {
      const response = await authApi.login(data)
      setUser({
        id: response.userId,
        username: response.username,
        bearerToken: response.bearerToken,
        createdAt: new Date().toISOString()
      })
      return true
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Invalid username or PIN.'
      return false
    } finally {
      isLoading.value = false
    }
  }

  function logout() {
    user.value = null
    localStorage.removeItem('user')
    localStorage.removeItem('token')
  }

  function clearError() {
    error.value = ''
  }

  return {
    user,
    isAuthenticated,
    error,
    isLoading,
    setUser,
    register,
    login,
    logout,
    clearError
  }
})
