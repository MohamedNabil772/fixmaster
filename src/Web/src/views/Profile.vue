<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '../stores/auth'
import api from '../services/api'

const authStore = useAuthStore()
const firstName = ref('')
const lastName = ref('')
const profilePictureUrl = ref('')
const isLoading = ref(false)
const fileInput = ref<HTMLInputElement | null>(null)

onMounted(() => {
  if (authStore.user) {
    firstName.value = authStore.user.firstName
    lastName.value = authStore.user.lastName
    profilePictureUrl.value = authStore.user.profilePictureUrl || ''
  }
})

const handleImageClick = () => {
  fileInput.value?.click()
}

const handleFileChange = async (event: Event) => {
  const file = (event.target as HTMLInputElement).files?.[0]
  if (file && authStore.user) {
    const formData = new FormData()
    formData.append('file', file)
    
    try {
      isLoading.value = true
      // 1. Upload to FileServer
      const uploadRes = await api.post('/api/media/uploads', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      })
      
      const imageUrl = uploadRes.data.url
      profilePictureUrl.value = imageUrl
      
      // 2. Update local state (user still needs to click Save for other info)
    } catch (error) {
      console.error('Failed to upload image:', error)
      alert('Upload failed')
    } finally {
      isLoading.value = false
    }
  }
}

const handleSubmit = async () => {
  isLoading.value = true
  try {
    await authStore.updateProfile(firstName.value, lastName.value, profilePictureUrl.value)
    alert('Profile updated successfully!')
  } catch (error) {
    console.error('Update failed', error)
    alert('Failed to update profile')
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="flex flex-wrap mt-4">
    <div class="w-full lg:w-8/12 px-4 mx-auto">
      <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-blueGray-100 border-0">
        <div class="rounded-t bg-white mb-0 px-6 py-6">
          <div class="text-center flex justify-between">
            <h6 class="text-blueGray-700 text-xl font-bold">My Account</h6>
            <button
              @click="handleSubmit"
              :disabled="isLoading"
              class="bg-emerald-500 text-white active:bg-emerald-600 font-bold uppercase text-xs px-4 py-2 rounded shadow hover:shadow-md outline-none focus:outline-none mr-1 ease-linear transition-all duration-150 disabled:opacity-50"
              type="button"
            >
              {{ isLoading ? 'Saving...' : 'Save Profile' }}
            </button>
          </div>
        </div>
        <div class="flex-auto px-4 lg:px-10 py-10 pt-0">
          <form @submit.prevent="handleSubmit">
            <h6 class="text-blueGray-400 text-sm mt-3 mb-6 font-bold uppercase">
              User Information
            </h6>
            <div class="flex flex-wrap">
              <div class="w-full lg:w-12/12 px-4 mb-6 flex justify-center">
                <div 
                  @click="handleImageClick"
                  class="relative w-32 h-32 rounded-full border-4 border-white shadow-md overflow-hidden cursor-pointer group"
                >
                  <img v-if="profilePictureUrl" :src="profilePictureUrl" class="w-full h-full object-cover" />
                  <div v-else class="w-full h-full bg-blue-50 flex items-center justify-center">
                    <i class="fas fa-user text-4xl text-blue-300"></i>
                  </div>
                  <div class="absolute inset-0 bg-black bg-opacity-40 opacity-0 group-hover:opacity-100 flex items-center justify-center transition-opacity">
                    <i class="fas fa-camera text-white text-xl"></i>
                  </div>
                  <input type="file" ref="fileInput" class="hidden" accept="image/*" @change="handleFileChange" />
                </div>
              </div>
              
              <div class="w-full lg:w-6/12 px-4">
                <div class="relative w-full mb-3">
                  <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
                    First Name
                  </label>
                  <input
                    v-model="firstName"
                    type="text"
                    class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none focus:ring w-full ease-linear transition-all duration-150"
                  />
                </div>
              </div>
              <div class="w-full lg:w-6/12 px-4">
                <div class="relative w-full mb-3">
                  <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
                    Last Name
                  </label>
                  <input
                    v-model="lastName"
                    type="text"
                    class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none focus:ring w-full ease-linear transition-all duration-150"
                  />
                </div>
              </div>
              <div class="w-full lg:w-12/12 px-4">
                <div class="relative w-full mb-3">
                  <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
                    Email Address
                  </label>
                  <input
                    :value="authStore.user?.email"
                    disabled
                    type="email"
                    class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-blueGray-200 rounded text-sm shadow focus:outline-none w-full ease-linear transition-all duration-150 cursor-not-allowed"
                  />
                </div>
              </div>
            </div>

            <hr class="mt-6 border-b-1 border-blueGray-300" />

            <h6 class="text-blueGray-400 text-sm mt-3 mb-6 font-bold uppercase">
              Role Details
            </h6>
            <div class="flex flex-wrap">
              <div class="w-full lg:w-12/12 px-4">
                <div class="relative w-full mb-3">
                  <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
                    Current Role
                  </label>
                  <div class="flex items-center gap-2">
                    <span class="px-3 py-1 bg-primary text-white font-bold rounded-md text-xs uppercase">
                      {{ authStore.user?.role }}
                    </span>
                    <p class="text-xs text-blueGray-400">Your account type defines your permissions in the system.</p>
                  </div>
                </div>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
