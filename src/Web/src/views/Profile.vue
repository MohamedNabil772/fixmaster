<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '../stores/auth'
import AppButton from '../components/common/AppButton.vue'
import AppInput from '../components/common/AppInput.vue'

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

const handleFileChange = (event: Event) => {
  const file = (event.target as HTMLInputElement).files?.[0]
  if (file) {
    // In a real app, we would upload to storage (S3/Azure)
    // For this prototype, we'll use a Base64 string or a fake URL
    const reader = new FileReader()
    reader.onload = (e) => {
      profilePictureUrl.value = e.target?.result as string
    }
    reader.readAsDataURL(file)
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
  <div class="max-w-4xl mx-auto py-10 px-4">
    <div class="bg-white shadow rounded-lg overflow-hidden">
      <div class="bg-primary px-6 py-8">
        <h2 class="text-2xl font-bold text-white">Edit Your Profile</h2>
        <p class="text-blue-100 text-sm">Manage your account information and preferences.</p>
      </div>

      <div class="p-8">
        <form @submit.prevent="handleSubmit" class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <!-- Image Section -->
          <div class="flex flex-col items-center space-y-4">
            <div 
              @click="handleImageClick"
              class="relative w-40 h-40 rounded-full border-4 border-gray-100 shadow-sm overflow-hidden cursor-pointer group"
            >
              <img 
                v-if="profilePictureUrl" 
                :src="profilePictureUrl" 
                class="w-full h-full object-cover"
              />
              <div v-else class="w-full h-full bg-blue-50 flex items-center justify-center">
                <i class="fas fa-user text-5xl text-blue-300"></i>
              </div>
              
              <div class="absolute inset-0 bg-black bg-opacity-40 opacity-0 group-hover:opacity-100 flex items-center justify-center transition-opacity">
                <i class="fas fa-camera text-white text-2xl"></i>
              </div>
            </div>
            <p class="text-xs text-gray-500 text-center">Click to change profile picture</p>
            <input 
              type="file" 
              ref="fileInput" 
              class="hidden" 
              accept="image/*"
              @change="handleFileChange"
            />
          </div>

          <!-- Info Section -->
          <div class="md:col-span-2 space-y-6">
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <AppInput
                v-model="firstName"
                label="First Name"
                placeholder="Your first name"
                required
              />
              <AppInput
                v-model="lastName"
                label="Last Name"
                placeholder="Your last name"
                required
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Email Address</label>
              <input 
                :value="authStore.user?.email" 
                disabled 
                class="w-full bg-gray-50 border border-gray-200 rounded-md px-4 py-2 text-gray-500 cursor-not-allowed"
              />
              <p class="mt-1 text-xs text-gray-400">Email address cannot be changed.</p>
            </div>

            <div class="pt-4 flex justify-end">
              <AppButton 
                type="submit" 
                :disabled="isLoading"
                class="px-8"
              >
                {{ isLoading ? 'Saving Changes...' : 'Save Profile' }}
              </AppButton>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
