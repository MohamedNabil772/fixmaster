<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import AppButton from '../components/common/AppButton.vue'
import AppInput from '../components/common/AppInput.vue'

const router = useRouter()
const authStore = useAuthStore()

const email = ref('')
const firstName = ref('')
const lastName = ref('')
const password = ref('')
const confirmPassword = ref('')
const role = ref('Client')
const isLoading = ref(false)

const handleRegister = async () => {
  if (password.value !== confirmPassword.value) {
    alert('Passwords do not match')
    return
  }
  
  isLoading.value = true
  try {
    await authStore.register(email.value, password.value, firstName.value, lastName.value, role.value)
    router.push('/dashboard')
  } catch (error) {
    console.error('Registration failed', error)
    alert('Registration failed')
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <form @submit.prevent="handleRegister" class="space-y-6">
    <div class="grid grid-cols-2 gap-4">
      <AppInput
        v-model="firstName"
        label="First Name"
        required
        placeholder="John"
      />
      <AppInput
        v-model="lastName"
        label="Last Name"
        required
        placeholder="Doe"
      />
    </div>

    <AppInput
      v-model="email"
      label="Email address"
      type="email"
      required
      placeholder="Enter your email"
    />

    <div class="space-y-2">
      <label class="block text-sm font-medium text-gray-700">Account Type</label>
      <div class="flex gap-6">
        <label class="flex items-center gap-2 cursor-pointer">
          <input type="radio" v-model="role" value="Client" class="w-4 h-4 text-primary border-gray-300 focus:ring-primary" />
          <span class="text-sm text-gray-700">Client (Need Repairs)</span>
        </label>
        <label class="flex items-center gap-2 cursor-pointer">
          <input type="radio" v-model="role" value="Master" class="w-4 h-4 text-primary border-gray-300 focus:ring-primary" />
          <span class="text-sm text-gray-700">Master (Service Provider)</span>
        </label>
      </div>
    </div>
    
    <AppInput
      v-model="password"
      label="Password"
      type="password"
      required
      placeholder="Create a password"
    />

    <AppInput
      v-model="confirmPassword"
      label="Confirm Password"
      type="password"
      required
      placeholder="Confirm your password"
    />

    <div class="flex items-center justify-between">
      <div class="text-sm">
        <router-link to="/login" class="font-medium text-accent hover:text-opacity-80">
          Already have an account? Sign in
        </router-link>
      </div>
    </div>

    <AppButton type="submit" :disabled="isLoading" class="w-full">
      {{ isLoading ? 'Creating account...' : 'Register' }}
    </AppButton>
  </form>
</template>
