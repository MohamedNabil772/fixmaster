<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import AppInput from '../components/common/AppInput.vue'

const router = useRouter()
const authStore = useAuthStore()

const firstName = ref('')
const lastName = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const role = ref('Client')
const isLoading = ref(false)
const emailError = ref('')

const isEmailValid = computed(() => {
  const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
  return re.test(String(email.value).toLowerCase())
})

const handleRegister = async () => {
  if (!isEmailValid.value) {
    emailError.value = 'Please enter a valid email address.'
    return
  }
  emailError.value = ''

  if (password.value !== confirmPassword.value) {
    alert('Passwords do not match')
    return
  }
  
  isLoading.value = true
  try {
    await authStore.register(email.value, password.value, firstName.value, lastName.value, role.value)
    if (authStore.user?.role === 'Admin' || authStore.user?.role === 'SuperAdmin') {
      router.push('/admin/dashboard')
    } else {
      router.push('/dashboard')
    }
  } catch (error) {
    console.error('Registration failed', error)
    alert('Registration failed. Please try again.')
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="container mx-auto px-4 h-full">
    <div class="flex content-center items-center justify-center h-full pt-16 md:pt-24">
      <div class="w-full lg:w-6/12 px-4">
        <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-blueGray-200 border-0">
          <div class="rounded-t mb-0 px-6 py-6 text-center">
            <h6 class="text-blueGray-500 text-sm font-bold uppercase tracking-wider mb-3">
              Create your account
            </h6>
            <hr class="mt-6 border-b-1 border-blueGray-300" />
          </div>
          <div class="flex-auto px-4 lg:px-10 py-10 pt-0">
            <form @submit.prevent="handleRegister">
              <div class="flex flex-wrap">
                <div class="w-full lg:w-6/12 px-4">
                  <AppInput
                    v-model="firstName"
                    label="First Name"
                    placeholder="John"
                    required
                  />
                </div>
                <div class="w-full lg:w-6/12 px-4">
                  <AppInput
                    v-model="lastName"
                    label="Last Name"
                    placeholder="Doe"
                    required
                  />
                </div>
              </div>

              <div class="px-4">
                <AppInput
                  v-model="email"
                  label="Email"
                  type="email"
                  placeholder="Email"
                  :error="emailError"
                  required
                />
              </div>

              <div class="relative w-full mb-6 px-4">
                <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">Account Type</label>
                <div class="flex gap-4">
                  <label class="inline-flex items-center cursor-pointer">
                    <input type="radio" v-model="role" value="Client" class="form-radio border-0 rounded text-blueGray-700 ml-1 w-5 h-5" />
                    <span class="ml-2 text-sm font-semibold text-blueGray-600">Client</span>
                  </label>
                  <label class="inline-flex items-center cursor-pointer">
                    <input type="radio" v-model="role" value="Master" class="form-radio border-0 rounded text-blueGray-700 ml-1 w-5 h-5" />
                    <span class="ml-2 text-sm font-semibold text-blueGray-600">Master</span>
                  </label>
                </div>
              </div>

              <div class="flex flex-wrap">
                <div class="w-full lg:w-6/12 px-4">
                  <AppInput
                    v-model="password"
                    label="Password"
                    type="password"
                    placeholder="Password"
                    required
                  />
                </div>
                <div class="w-full lg:w-6/12 px-4">
                  <AppInput
                    v-model="confirmPassword"
                    label="Confirm Password"
                    type="password"
                    placeholder="Confirm Password"
                    required
                  />
                </div>
              </div>

              <div class="text-center mt-6">
                <button
                  class="bg-blueGray-800 text-white active:bg-blueGray-600 text-sm font-bold uppercase px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 w-full ease-linear transition-all duration-150 disabled:opacity-50"
                  type="submit"
                  :disabled="isLoading"
                >
                  {{ isLoading ? 'Creating Account...' : 'Create Account' }}
                </button>
              </div>
            </form>
          </div>
        </div>
        <div class="text-center mt-6">
          <router-link to="/auth/login" class="text-blueGray-200 hover:text-white transition-colors">
            <small>Already have an account? Sign in</small>
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>
