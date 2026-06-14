<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import AppInput from '../components/common/AppInput.vue'

const router = useRouter()
const authStore = useAuthStore()

const email = ref('')
const password = ref('')
const isLoading = ref(false)
const emailError = ref('')

const isEmailValid = computed(() => {
  const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
  return re.test(String(email.value).toLowerCase())
})

const handleLogin = async () => {
  if (!isEmailValid.value) {
    emailError.value = 'Please enter a valid email address.'
    return
  }
  emailError.value = ''
  
  isLoading.value = true
  try {
    await authStore.login(email.value, password.value)
    if (authStore.user?.role === 'Admin' || authStore.user?.role === 'SuperAdmin') {
      router.push('/admin/dashboard')
    } else {
      router.push('/dashboard')
    }
  } catch (error) {
    console.error('Login failed', error)
    alert('Login failed. Please check your credentials.')
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="container mx-auto px-4 h-full">
    <div class="flex content-center items-center justify-center h-full pt-16 md:pt-32">
      <div class="w-full lg:w-4/12 px-4">
        <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-blueGray-200 border-0">
          <div class="rounded-t mb-0 px-6 py-6 text-center">
            <h6 class="text-blueGray-500 text-sm font-bold uppercase tracking-wider mb-3">
              Sign in with credentials
            </h6>
            <hr class="mt-6 border-b-1 border-blueGray-300" />
          </div>
          <div class="flex-auto px-4 lg:px-10 py-10 pt-0">
            <form @submit.prevent="handleLogin">
              <AppInput
                v-model="email"
                label="Email"
                type="email"
                placeholder="Email"
                :error="emailError"
                required
              />

              <AppInput
                v-model="password"
                label="Password"
                type="password"
                placeholder="Password"
                required
              />

              <div class="text-center mt-6">
                <button
                  class="bg-blueGray-800 text-white active:bg-blueGray-600 text-sm font-bold uppercase px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 w-full ease-linear transition-all duration-150 disabled:opacity-50"
                  type="submit"
                  :disabled="isLoading"
                >
                  {{ isLoading ? 'Signing In...' : 'Sign In' }}
                </button>
              </div>
            </form>
          </div>
        </div>
        <div class="flex flex-wrap mt-6 relative">
          <div class="w-1/2">
            <a href="javascript:void(0)" class="text-blueGray-200 hover:text-white transition-colors">
              <small>Forgot password?</small>
            </a>
          </div>
          <div class="w-1/2 text-right">
            <router-link to="/auth/register" class="text-blueGray-200 hover:text-white transition-colors">
              <small>Create new account</small>
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
