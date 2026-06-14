<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import { useBiddingStore } from '../../stores/bidding'

const biddingStore = useBiddingStore()
const statusFilter = ref('')
const currentPage = ref(1)
const pageSize = ref(10)

const fetchBids = () => {
  biddingStore.fetchAllBids(currentPage.value, pageSize.value, statusFilter.value || undefined)
}

onMounted(() => {
  fetchBids()
})

watch([statusFilter, currentPage], () => {
  fetchBids()
})

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
                Bids Management
              </h3>
            </div>
            <div class="relative w-full px-4 max-w-full flex-grow flex-1 text-right flex items-center justify-end gap-4">
              <span class="text-xs font-bold text-blueGray-400 uppercase">Filter Status:</span>
              <select 
                v-model="statusFilter"
                class="border border-blueGray-200 px-3 py-2 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow-sm outline-none focus:outline-none focus:ring w-48"
              >
                <option value="">All Statuses</option>
                <option value="Pending">Pending</option>
                <option value="Accepted">Accepted</option>
                <option value="Rejected">Rejected</option>
              </select>
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
                  Master ID
                </th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">
                  Date
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="bid in biddingStore.allBids?.items" :key="bid.id">
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                  <span class="font-bold text-blue-600 cursor-pointer hover:underline">
                    {{ bid.requestId.slice(0, 8) }}
                  </span>
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 font-bold text-blueGray-600">
                  ${{ bid.amount.toLocaleString() }}
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
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 text-blueGray-400">
                  {{ bid.masterId.slice(0, 8) }}...
                </td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">
                  {{ formatDate(bid.createdAt) }}
                </td>
              </tr>
              <tr v-if="!biddingStore.allBids?.items.length && !biddingStore.isLoading">
                <td colspan="5" class="text-center py-12 text-blueGray-400 italic">
                  No bids found matching the criteria.
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        
        <!-- Pagination -->
        <div class="px-4 py-3 border-t border-blueGray-100 bg-blueGray-50 flex items-center justify-between">
          <div class="text-xs text-blueGray-500 font-bold uppercase">
            Showing {{ ((currentPage - 1) * pageSize) + 1 }} to {{ Math.min(currentPage * pageSize, biddingStore.allBids?.totalCount || 0) }} of {{ biddingStore.allBids?.totalCount }} entries
          </div>
          <div class="flex gap-2">
            <button 
              @click="currentPage--"
              :disabled="!biddingStore.allBids?.hasPreviousPage"
              class="px-3 py-1 bg-white border border-blueGray-200 rounded text-xs font-bold text-blueGray-600 disabled:opacity-50 disabled:cursor-not-allowed hover:bg-blueGray-100"
            >
              Previous
            </button>
            <button 
              @click="currentPage++"
              :disabled="!biddingStore.allBids?.hasNextPage"
              class="px-3 py-1 bg-white border border-blueGray-200 rounded text-xs font-bold text-blueGray-600 disabled:opacity-50 disabled:cursor-not-allowed hover:bg-blueGray-100"
            >
              Next
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
