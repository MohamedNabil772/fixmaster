<template>
  <div>
    <a
      class="text-blueGray-500 block"
      href="javascript:void(0)"
      ref="btnDropdownRef"
      v-on:click="toggleDropdown($event)"
    >
      <div class="items-center flex">
        <span
          class="w-10 h-10 text-sm text-white bg-blue-100 inline-flex items-center justify-center rounded-full border border-blue-200 overflow-hidden"
        >
          <img v-if="authStore.user?.profilePictureUrl" :src="authStore.user.profilePictureUrl" class="w-full h-full object-cover" />
          <i v-else class="fas fa-user text-blue-600"></i>
        </span>
      </div>
    </a>
    <div
      ref="popoverDropdownRef"
      class="bg-white text-base z-50 float-left py-2 list-none text-left rounded shadow-lg min-w-56"
      v-bind:class="{
        hidden: !dropdownPopoverShow,
        block: dropdownPopoverShow,
      }"
    >
      <div class="px-4 py-4 border-b border-solid border-blueGray-100 mb-2 flex items-center gap-3">
        <div 
          class="relative w-12 h-12 rounded-full overflow-hidden cursor-pointer group flex-shrink-0"
          @click="handleImageClick"
        >
          <img v-if="authStore.user?.profilePictureUrl" :src="authStore.user.profilePictureUrl" class="w-full h-full object-cover" />
          <div v-else class="w-full h-full bg-blue-50 flex items-center justify-center">
            <i class="fas fa-user text-blue-300"></i>
          </div>
          <div class="absolute inset-0 bg-black bg-opacity-40 opacity-0 group-hover:opacity-100 flex items-center justify-center transition-opacity">
            <i class="fas fa-camera text-white text-xs"></i>
          </div>
          <input type="file" ref="fileInput" class="hidden" accept="image/*" @change="handleFileChange" />
        </div>
        <div class="min-w-0">
          <p class="text-sm font-bold text-blueGray-700 truncate leading-tight">{{ authStore.user?.firstName }} {{ authStore.user?.lastName }}</p>
          <p class="text-[10px] text-blueGray-400 truncate">{{ authStore.user?.email }}</p>
        </div>
      </div>
      
      <router-link
        to="/profile"
        class="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700 hover:bg-blueGray-100"
        @click="dropdownPopoverShow = false"
      >
        <i class="fas fa-id-card mr-2 text-blueGray-400"></i>
        My Profile
      </router-link>

      <div class="h-0 my-2 border border-solid border-blueGray-100" />
      
      <a
        href="javascript:void(0);"
        class="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-red-600 hover:bg-red-50"
        @click="handleLogout"
      >
        <i class="fas fa-sign-out-alt mr-2"></i>
        Logout
      </a>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { createPopper } from "@popperjs/core"
import { useAuthStore } from "../../stores/auth"
import api from '../../services/api'

const router = useRouter()
const authStore = useAuthStore()

const dropdownPopoverShow = ref(false)
const btnDropdownRef = ref<HTMLElement | null>(null)
const popoverDropdownRef = ref<HTMLElement | null>(null)
const fileInput = ref<HTMLInputElement | null>(null)

const toggleDropdown = (event: Event) => {
  event.preventDefault()
  if (dropdownPopoverShow.value) {
    dropdownPopoverShow.value = false
  } else {
    dropdownPopoverShow.value = true
    if (btnDropdownRef.value && popoverDropdownRef.value) {
      createPopper(btnDropdownRef.value, popoverDropdownRef.value, {
        placement: "bottom-end",
      })
    }
  }
}

const handleImageClick = () => {
  fileInput.value?.click()
}

const handleFileChange = async (event: Event) => {
  const file = (event.target as HTMLInputElement).files?.[0]
  if (file && authStore.user) {
    const formData = new FormData()
    formData.append('file', file)
    
    try {
      // 1. Upload to FileServer
      const uploadRes = await api.post('/api/media/uploads', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      })
      
      const imageUrl = uploadRes.data.url
      
      // 2. Update Profile in Identity Service
      await authStore.updateProfile(
        authStore.user.firstName, 
        authStore.user.lastName, 
        imageUrl
      )
      
      alert('Profile picture updated!')
    } catch (error) {
      console.error('Failed to update profile picture:', error)
      alert('Upload failed')
    }
  }
}

const handleLogout = () => {
  dropdownPopoverShow.value = false
  authStore.logout()
  router.push('/login')
}
</script>
