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
const isLoading = ref(false)

const handleRegister = async () => {
  if (password.value !== confirmPassword.value) {
    alert('Passwords do not match')
    return
  }
  
  isLoading.value = true
  try {
    await authStore.register(email.value, password.value, firstName.value, lastName.value)
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
