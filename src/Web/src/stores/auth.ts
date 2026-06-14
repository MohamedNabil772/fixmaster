import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '../services/api'

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  profilePictureUrl?: string;
}

export interface AuthResponse extends User {
  token: string;
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(JSON.parse(localStorage.getItem('user') || 'null'))
  const token = ref<string | null>(localStorage.getItem('token'))
  const allUsers = ref<User[]>([])
  const isAuthenticated = computed(() => !!token.value)
  const isSuperAdmin = computed(() => user.value?.role === 'SuperAdmin')

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

    const { token: newToken, ...userData } = response.data
    setToken(newToken)
    setUser(userData)
  }

  async function register(email: string, password: string, firstName: string, lastName: string, role: string = 'Client') {
    const response = await api.post<AuthResponse>('/api/identity/users/register', {
      email,
      password,
      firstName,
      lastName,
      role
    })

    const { token: newToken, ...userData } = response.data
    setToken(newToken)
    setUser(userData)
  }

  async function fetchAllUsers() {
    const response = await api.get<User[]>('/api/identity/users')
    allUsers.value = response.data
  }

  async function updateUserRole(userId: string, newRole: string) {
    await api.post(`/api/identity/users/${userId}/role`, `"${newRole}"`, {
      headers: { 'Content-Type': 'application/json' }
    })
    await fetchAllUsers()
  }

  async function updateProfile(firstName: string, lastName: string, profilePictureUrl?: string) {
    await api.put('/api/identity/users/profile', {
      firstName,
      lastName,
      profilePictureUrl
    })
    
    if (user.value) {
      const updatedUser = { ...user.value, firstName, lastName, profilePictureUrl }
      setUser(updatedUser)
    }
  }

  return {
    user,
    token,
    allUsers,
    isAuthenticated,
    isSuperAdmin,
    setToken,
    setUser,
    logout,
    login,
    register,
    fetchAllUsers,
    updateUserRole,
    updateProfile
  }
})
