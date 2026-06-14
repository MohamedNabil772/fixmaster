<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()
const isSidebarOpen = ref(false)

const handleLogout = () => {
  authStore.logout()
  router.push('/login')
}
</script>

<template>
  <div class="min-h-screen bg-gray-100 flex flex-col">
    <!-- Header -->
    <header class="bg-white shadow-sm h-16 flex items-center justify-between px-4 lg:px-8 sticky top-0 z-10">
      <div class="flex items-center gap-4">
        <button @click="isSidebarOpen = !isSidebarOpen" class="lg:hidden p-2">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
          </svg>
        </button>
        <h1 class="text-xl font-bold text-primary">FixMaster</h1>
      </div>
      
      <div class="flex items-center gap-4">
        <span class="text-sm font-medium hidden md:inline">{{ authStore.user?.email }}</span>
        <button @click="handleLogout" class="text-sm text-accent font-medium hover:underline">
          Logout
        </button>
      </div>
    </header>

    <div class="flex-1 flex overflow-hidden">
      <!-- Sidebar (Desktop) -->
      <aside class="hidden lg:flex w-64 bg-secondary flex-col">
        <nav class="flex-1 px-4 py-6 space-y-2">
          <router-link to="/dashboard" class="block px-4 py-2 rounded-md text-white hover:bg-primary transition-colors" active-class="bg-primary">
            Dashboard
          </router-link>
          <!-- More links can be added here -->
        </nav>
      </aside>

      <!-- Main Content -->
      <main class="flex-1 overflow-y-auto p-4 lg:p-8">
        <router-view />
      </main>
    </div>

    <!-- Mobile Sidebar Backdrop -->
    <div v-if="isSidebarOpen" @click="isSidebarOpen = false" class="fixed inset-0 bg-black bg-opacity-50 lg:hidden z-20"></div>
    
    <!-- Mobile Sidebar -->
    <aside 
      class="fixed inset-y-0 left-0 w-64 bg-secondary transform transition-transform duration-300 lg:hidden z-30"
      :class="isSidebarOpen ? 'translate-x-0' : '-translate-x-full'"
    >
      <div class="p-4 border-b border-primary flex justify-between items-center">
        <span class="text-white font-bold">Menu</span>
        <button @click="isSidebarOpen = false" class="text-white">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
      <nav class="px-4 py-6 space-y-2">
        <router-link to="/dashboard" @click="isSidebarOpen = false" class="block px-4 py-2 rounded-md text-white hover:bg-primary" active-class="bg-primary">
          Dashboard
        </router-link>
      </nav>
    </aside>
  </div>
</template>
