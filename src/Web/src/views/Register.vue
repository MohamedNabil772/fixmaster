<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const firstName = ref('')
const lastName = ref('')
const email = ref('')
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
  <div class="container mx-auto px-4 h-full">
    <div class="flex content-center items-center justify-center h-full pt-16">
      <div class="w-full lg:w-6/12 px-4">
        <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-blueGray-200 border-0">
          <div class="rounded-t mb-0 px-6 py-6">
            <div class="text-center mb-3">
              <h6 class="text-blueGray-500 text-sm font-bold uppercase tracking-wider">
                Create your account
              </h6>
            </div>
            <hr class="mt-6 border-b-1 border-blueGray-300" />
          </div>
          <div class="flex-auto px-4 lg:px-10 py-10 pt-0">
            <form @submit.prevent="handleRegister">
              <div class="flex flex-wrap">
                <div class="w-full lg:w-6/12 px-4">
                  <div class="relative w-full mb-3">
                    <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">First Name</label>
                    <input v-model="firstName" type="text" class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none w-full" placeholder="John" required />
                  </div>
                </div>
                <div class="w-full lg:w-6/12 px-4">
                  <div class="relative w-full mb-3">
                    <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">Last Name</label>
                    <input v-model="lastName" type="text" class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none w-full" placeholder="Doe" required />
                  </div>
                </div>
              </div>

              <div class="relative w-full mb-3 px-4">
                <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">Email</label>
                <input v-model="email" type="email" class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none w-full" placeholder="Email" required />
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
                  <div class="relative w-full mb-3">
                    <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">Password</label>
                    <input v-model="password" type="password" class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none w-full" placeholder="Password" required />
                  </div>
                </div>
                <div class="w-full lg:w-6/12 px-4">
                  <div class="relative w-full mb-3">
                    <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">Confirm Password</label>
                    <input v-model="confirmPassword" type="password" class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none w-full" placeholder="Confirm Password" required />
                  </div>
                </div>
              </div>

              <div class="text-center mt-6">
                <button
                  class="bg-blueGray-800 text-white active:bg-blueGray-600 text-sm font-bold uppercase px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 w-full ease-linear transition-all duration-150"
                  type="submit"
                  :disabled="isLoading"
                >
                  {{ isLoading ? 'Creating Account...' : 'Create Account' }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
