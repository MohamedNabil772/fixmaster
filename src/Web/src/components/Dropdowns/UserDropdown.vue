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
          class="w-10 h-10 text-sm text-white bg-blue-100 inline-flex items-center justify-center rounded-full border border-blue-200"
        >
          <i class="fas fa-user text-blue-600"></i>
        </span>
      </div>
    </a>
    <div
      ref="popoverDropdownRef"
      class="bg-white text-base z-50 float-left py-2 list-none text-left rounded shadow-lg min-w-48"
      v-bind:class="{
        hidden: !dropdownPopoverShow,
        block: dropdownPopoverShow,
      }"
    >
      <div class="px-4 py-2 border-b border-solid border-blueGray-100 mb-2">
        <p class="text-sm font-bold text-blueGray-700 truncate">{{ authStore.user?.firstName }} {{ authStore.user?.lastName }}</p>
        <p class="text-xs text-blueGray-500 truncate">{{ authStore.user?.email }}</p>
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

const router = useRouter()
const authStore = useAuthStore()

const dropdownPopoverShow = ref(false)
const btnDropdownRef = ref<HTMLElement | null>(null)
const popoverDropdownRef = ref<HTMLElement | null>(null)

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

const handleLogout = () => {
  dropdownPopoverShow.value = false
  authStore.logout()
  router.push('/login')
}
</script>
