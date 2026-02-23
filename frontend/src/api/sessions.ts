import apiClient from './client'
import type { PomodoroSession } from '@/types'

export interface CreateSessionRequest {
  type: 'pomodoro' | 'short_break' | 'long_break'
  durationSeconds: number
  isCompleted: boolean
  completedAt: string
}

export interface SessionHistoryParams {
  startDate?: string
  endDate?: string
  limit?: number
}

export const sessionsApi = {
  async createSession(data: CreateSessionRequest): Promise<{ sessionId: number }> {
    const response = await apiClient.post<{ sessionId: number }>('/api/sessions', data)
    return response.data
  },

  async getSessionHistory(params?: SessionHistoryParams): Promise<PomodoroSession[]> {
    const response = await apiClient.get<PomodoroSession[]>('/api/sessions', { params })
    return response.data
  }
}
