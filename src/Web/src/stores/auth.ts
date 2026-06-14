import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '../services/api'

export interface User {
  id: string;
  email: string;
  role: string;
}

export interface AuthResponse {
  user: User;
  token: string;
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(JSON.parse(localStorage.getItem('user') || 'null'))
  const token = ref<string | null>(localStorage.getItem('token'))
  const isAuthenticated = computed(() => !!token.value)

  function setToken(newToken: string) {
    token.value = newToken
    localStorage.setItem('token', newToken)
  }

  function setUser(newUser: User) {
    user.value = newUser
    localStorage.setItem('user', JSON.stringify(newUser))
  }

  function logout() {
    user.value = null
    token.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  async function login(email: string, password: string) {
    const response = await api.post<AuthResponse>('/api/identity/users/login', {
      email,
      password
    })
    
    const { token: newToken, user: newUser } = response.data
    setToken(newToken)
    setUser(newUser)
  }

  async function register(email: string, password: string, firstName: string, lastName: string) {
    const response = await api.post<AuthResponse>('/api/identity/users/register', {
      email,
      password,
      firstName,
      lastName
    })
    
    const { token: newToken, user: newUser } = response.data
    setToken(newToken)
    setUser(newUser)
  }

  return {
    user,
    token,
    isAuthenticated,
    setToken,
    setUser,
    logout,
    login,
    register
  }
})
