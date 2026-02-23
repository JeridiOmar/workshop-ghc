import apiClient from './client'

export interface RegisterRequest {
  username: string
  pin: string
}

export interface LoginRequest {
  username: string
  pin: string
}

export interface AuthResponse {
  userId: string
  username: string
  bearerToken: string
}

export const authApi = {
  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await apiClient.post<AuthResponse>('/api/auth/register', data)
    return response.data
  },

  async login(data: LoginRequest): Promise<AuthResponse> {
    const response = await apiClient.post<AuthResponse>('/api/auth/login', data)
    return response.data
  }
}
