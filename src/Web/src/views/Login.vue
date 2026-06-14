<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import AppButton from '../components/common/AppButton.vue'
import AppInput from '../components/common/AppInput.vue'

const router = useRouter()
const authStore = useAuthStore()

const email = ref('')
const password = ref('')
const isLoading = ref(false)

const handleLogin = async () => {
  isLoading.value = true
  try {
    await authStore.login(email.value, password.value)
    router.push('/dashboard')
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <form @submit.prevent="handleLogin" class="space-y-6">
    <AppInput
      v-model="email"
      label="Email address"
      type="email"
      required
      placeholder="Enter your email"
    />
    
    <AppInput
      v-model="password"
      label="Password"
      type="password"
      required
      placeholder="Enter your password"
    />

    <div class="flex items-center justify-between">
      <div class="text-sm">
        <router-link to="/register" class="font-medium text-accent hover:text-opacity-80">
          Don't have an account? Register
        </router-link>
      </div>
    </div>

    <AppButton type="submit" :disabled="isLoading" class="w-full">
      {{ isLoading ? 'Signing in...' : 'Sign in' }}
    </AppButton>
  </form>
</template>
