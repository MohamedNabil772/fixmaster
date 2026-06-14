<script setup lang="ts">
import { onMounted, computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useBiddingStore } from '../stores/bidding'
import { useAuthStore } from '../stores/auth'
import AdminDashboard from './admin/Dashboard.vue'

const router = useRouter()
const biddingStore = useBiddingStore()
const authStore = useAuthStore()

const isClient = computed(() => authStore.user?.role === 'Client')
const isAdmin = computed(() => authStore.user?.role === 'Admin' || authStore.user?.role === 'SuperAdmin')

onMounted(() => {
  if (isClient.value) {
    biddingStore.fetchServiceRequests()
  } else if (isAdmin.value) {
    // Admin dashboard logic is inside AdminDashboard component
  } else {
    biddingStore.fetchMyBids()
  }
})

function navigateToRequest(id: string) {
  router.push(`/requests/${id}`)
}

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString()
}
</script>

<template>
  <div class="flex flex-wrap mt-4">
    <!-- Admin/SuperAdmin Dashboard -->
    <template v-if="isAdmin">
      <div class="w-full px-4">
        <AdminDashboard />
      </div>
    </template>

    <!-- Client Dashboard -->
    <template v-else-if="isClient">
      <div class="w-full px-4">
        <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded bg-white">
          <div class="rounded-t mb-0 px-4 py-3 border-0">
            <div class="flex flex-wrap items-center">
              <div class="relative w-full px-4 max-w-full flex-grow flex-1">
                <h3 class="font-semibold text-lg text-blueGray-700">
                  Your Service Requests
                </h3>
              </div>
              <div class="relative w-full px-4 max-w-full flex-grow flex-1 text-right">
                <button 
                  @click="router.push('/post-request')"
                  class="bg-blue-500 text-white active:bg-blue-600 font-bold uppercase text-xs px-4 py-2 rounded shadow hover:shadow-md outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                >
                  New Request
                </button>
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
                    Budget
                  </th>
                  <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                    Status
                  </th>
                  <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                    Created At
                  </th>
                  <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="request in biddingStore.serviceRequests" :key="request.id">
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 font-bold text-blueGray-600">
                    {{ request.title }}
                  </td>
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                    ${{ request.budget }}
                  </td>
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                    <span class="px-2 py-1 rounded text-[10px] font-bold uppercase"
                      :class="{
                        'bg-orange-100 text-orange-600': request.status === 'Open',
                        'bg-blue-100 text-blue-600': request.status === 'InProgress',
                        'bg-emerald-100 text-emerald-600': request.status === 'Completed'
                      }">
                      {{ request.status }}
                    </span>
                  </td>
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                    {{ formatDate(request.createdAt) }}
                  </td>
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 text-right">
                    <button 
                      @click="navigateToRequest(request.id)"
                      class="text-blue-500 font-bold hover:underline"
                    >
                      Details
                    </button>
                  </td>
                </tr>
                <tr v-if="biddingStore.serviceRequests.length === 0">
                  <td colspan="5" class="text-center py-12 text-blueGray-400 italic">
                    No service requests found.
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </template>

    <!-- Master Dashboard -->
    <template v-else>
      <div class="w-full px-4">
        <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded bg-white">
          <div class="rounded-t mb-0 px-4 py-3 border-0">
            <div class="flex flex-wrap items-center">
              <div class="relative w-full px-4 max-w-full flex-grow flex-1">
                <h3 class="font-semibold text-lg text-blueGray-700">
                  Your Bids
                </h3>
              </div>
              <div class="relative w-full px-4 max-w-full flex-grow flex-1 text-right">
                <button 
                  @click="router.push('/browse-requests')"
                  class="bg-emerald-500 text-white active:bg-emerald-600 font-bold uppercase text-xs px-4 py-2 rounded shadow hover:shadow-md outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                >
                  Browse Requests
                </button>
              </div>
            </div>
          </div>
          <div class="block w-full overflow-x-auto">
            <table class="items-center w-full bg-transparent border-collapse">
              <thead>
                <tr>
                  <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                    Request ID
                  </th>
                  <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                    Amount
                  </th>
                  <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                    Status
                  </th>
                  <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                    Date
                  </th>
                  <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="bid in biddingStore.myBids" :key="bid.id">
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                    {{ bid.requestId.slice(0, 8) }}
                  </td>
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 font-bold text-blueGray-600">
                    ${{ bid.amount }}
                  </td>
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                    <span class="px-2 py-1 rounded text-[10px] font-bold uppercase"
                      :class="{
                        'bg-orange-100 text-orange-600': bid.status === 'Pending',
                        'bg-emerald-100 text-emerald-600': bid.status === 'Accepted',
                        'bg-red-100 text-red-600': bid.status === 'Rejected'
                      }">
                      {{ bid.status }}
                    </span>
                  </td>
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                    {{ formatDate(bid.createdAt) }}
                  </td>
                  <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 text-right">
                    <button 
                      @click="navigateToRequest(bid.requestId)"
                      class="text-blue-500 font-bold hover:underline"
                    >
                      View Request
                    </button>
                  </td>
                </tr>
                <tr v-if="biddingStore.myBids.length === 0">
                  <td colspan="5" class="text-center py-12 text-blueGray-400 italic">
                    You haven't placed any bids yet.
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>
