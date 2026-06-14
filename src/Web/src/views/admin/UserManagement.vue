<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useAuthStore } from '../../stores/auth'

const authStore = useAuthStore()
const isLoading = ref(false)

const fetchData = async () => {
  isLoading.value = true
  try {
    await authStore.fetchAllUsers()
  } catch (error) {
    console.error('Failed to fetch users', error)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchData()
})

const roles = ['SuperAdmin', 'Admin', 'Client', 'Master']

const handleRoleChange = async (userId: string, event: Event) => {
  const newRole = (event.target as HTMLSelectElement).value
  try {
    await authStore.updateUserRole(userId, newRole)
    alert('User role updated successfully')
  } catch (error) {
    console.error('Failed to update role', error)
    alert('Failed to update role')
  }
}
</script>

<template>
  <div class="flex flex-wrap mt-4">
    <div class="w-full mb-12 px-4">
      <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded bg-white">
        <div class="rounded-t mb-0 px-4 py-3 border-0">
          <div class="flex flex-wrap items-center">
            <div class="relative w-full px-4 max-w-full flex-grow flex-1">
              <h3 class="font-semibold text-lg text-blueGray-700">
                User & Role Management
              </h3>
            </div>
            <div class="relative w-full px-4 max-w-full flex-grow flex-1 text-right">
              <button 
                @click="fetchData"
                class="bg-blue-500 text-white active:bg-blue-600 font-bold uppercase text-xs px-4 py-2 rounded shadow hover:shadow-md outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
              >
                Refresh
              </button>
            </div>
          </div>
        </div>
        <div class="block w-full overflow-x-auto">
          <table class="items-center w-full bg-transparent border-collapse">
            <thead>
              <tr>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  User
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Email
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Current Role
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="u in authStore.allUsers" :key="u.id">
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 font-bold text-blueGray-600">
                  {{ u.firstName }} {{ u.lastName }}
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                  {{ u.email }}
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                   <span class="px-2 py-1 rounded text-white font-bold text-[10px] uppercase"
                    :class="{
                      'bg-red-500': u.role === 'SuperAdmin',
                      'bg-orange-500': u.role === 'Admin',
                      'bg-emerald-500': u.role === 'Master',
                      'bg-blue-500': u.role === 'Client'
                    }">
                    {{ u.role }}
                  </span>
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                  <select 
                    @change="handleRoleChange(u.id, $event)"
                    :value="u.role"
                    class="border border-gray-300 px-3 py-1 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-xs shadow-sm outline-none focus:outline-none focus:ring w-32"
                  >
                    <option v-for="role in roles" :key="role" :value="role">
                      {{ role }}
                    </option>
                  </select>
                </td>
              </tr>
              <tr v-if="authStore.allUsers.length === 0 && !isLoading">
                <td colspan="4" class="text-center py-8 text-blueGray-500 italic">
                  No users found.
                </td>
              </tr>
              <tr v-if="isLoading">
                <td colspan="4" class="text-center py-8">
                  <span class="text-blueGray-500">Loading users...</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
