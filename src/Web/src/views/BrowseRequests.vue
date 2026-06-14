<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useBiddingStore } from '../stores/bidding'

const router = useRouter()
const biddingStore = useBiddingStore()
const requests = ref<any[]>([])
const categoryFilter = ref('')

const categories = ['Plumbing', 'Electrical', 'Carpentry', 'Painting', 'Cleaning', 'Other']

const fetchRequests = async () => {
  // For simplicity, we use the existing store action.
  // In a real app, this would have filters.
  await biddingStore.fetchServiceRequests()
  requests.value = biddingStore.serviceRequests
}

onMounted(() => {
  fetchRequests()
})

const navigateToRequest = (id: string) => {
  router.push(`/requests/${id}`)
}

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString()
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
                Browse Available Requests
              </h3>
            </div>
            <div class="relative w-full px-4 max-w-full flex-grow flex-1 text-right flex items-center justify-end gap-4">
              <span class="text-xs font-bold text-blueGray-400 uppercase">Category:</span>
              <select 
                v-model="categoryFilter"
                class="border border-blueGray-200 px-3 py-2 text-blueGray-600 bg-white rounded text-sm shadow-sm outline-none focus:ring w-48"
              >
                <option value="">All Categories</option>
                <option v-for="cat in categories" :key="cat" :value="cat">{{ cat }}</option>
              </select>
            </div>
          </div>
        </div>
        <div class="block w-full overflow-x-auto">
          <table class="items-center w-full bg-transparent border-collapse">
            <thead>
              <tr>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Title
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Category
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Budget
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Status
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Posted
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="req in requests" :key="req.id">
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 font-bold text-blueGray-600">
                  {{ req.title }}
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                  {{ req.category }}
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 font-bold text-emerald-600">
                  ${{ req.budget }}
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                  <span class="px-2 py-1 rounded text-[10px] font-bold uppercase bg-blue-100 text-blue-600">
                    {{ req.status }}
                  </span>
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                  {{ formatDate(req.createdAt) }}
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 text-right">
                  <button 
                    @click="navigateToRequest(req.id)"
                    class="bg-blue-500 text-white font-bold uppercase text-[10px] px-3 py-1 rounded shadow hover:shadow-md transition-all duration-150"
                  >
                    View & Bid
                  </button>
                </td>
              </tr>
              <tr v-if="requests.length === 0">
                <td colspan="6" class="text-center py-12 text-blueGray-400 italic">
                  No requests available at the moment.
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
