<script setup lang="ts">
import { onMounted, ref, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useBiddingStore, type Bid } from '../stores/bidding'
import { useAuthStore } from '../stores/auth'

const route = useRoute()
const router = useRouter()
const biddingStore = useBiddingStore()
const authStore = useAuthStore()

const requestId = route.params.id as string
const request = ref<any>(null)
const bids = ref<Bid[]>([])
const bidAmount = ref<number>(0)
const bidDescription = ref('')
const isSubmitting = ref(false)

const isClient = computed(() => authStore.user?.role === 'Client')
const isMaster = computed(() => authStore.user?.role === 'Master')

onMounted(async () => {
  request.value = await biddingStore.fetchRequestDetails(requestId)
  bids.value = await biddingStore.fetchBids(requestId)
})

const handleBidSubmit = async () => {
  isSubmitting.value = true
  try {
    await biddingStore.submitBid({
      requestId,
      amount: bidAmount.value,
      description: bidDescription.value
    })
    bids.value = await biddingStore.fetchBids(requestId)
    bidAmount.value = 0
    bidDescription.value = ''
    alert('Bid submitted successfully!')
  } catch (error) {
    alert('Failed to submit bid')
  } finally {
    isSubmitting.value = false
  }
}

const handleAcceptBid = async (bidId: string) => {
  try {
    await biddingStore.acceptBid(requestId, bidId)
    request.value = await biddingStore.fetchRequestDetails(requestId)
    bids.value = await biddingStore.fetchBids(requestId)
    alert('Bid accepted!')
  } catch (error) {
    alert('Failed to accept bid')
  }
}

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString()
}
</script>

<template>
  <div class="flex flex-wrap mt-4">
    <div class="w-full lg:w-8/12 px-4 mx-auto" v-if="request">
      <!-- Request Info Card -->
      <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-white border-0">
        <div class="rounded-t mb-0 px-6 py-6 bg-blueGray-50">
          <div class="text-center flex justify-between">
            <h6 class="text-blueGray-700 text-xl font-bold">Request Details</h6>
            <span class="px-3 py-1 rounded-full text-xs font-bold uppercase"
              :class="{
                'bg-orange-100 text-orange-600': request.status === 'Open',
                'bg-blue-100 text-blue-600': request.status === 'InProgress',
                'bg-emerald-100 text-emerald-600': request.status === 'Completed'
              }">
              {{ request.status }}
            </span>
          </div>
        </div>
        <div class="flex-auto px-6 py-8">
          <h3 class="text-2xl font-bold text-blueGray-700 mb-2">{{ request.title }}</h3>
          <div class="flex items-center gap-4 mb-6">
            <span class="text-sm font-bold text-blueGray-400 uppercase tracking-wider">{{ request.category }}</span>
            <span class="text-sm font-bold text-emerald-500">Budget: ${{ request.budget }}</span>
          </div>
          <p class="text-blueGray-500 leading-relaxed mb-6">{{ request.description }}</p>
          <div class="text-xs text-blueGray-400">Posted on: {{ formatDate(request.createdAt) }}</div>
        </div>
      </div>

      <!-- Bid Submission (For Masters) -->
      <div v-if="isMaster && request.status === 'Open'" class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-emerald-50 border-0">
        <div class="flex-auto p-6">
          <h6 class="text-emerald-700 text-sm font-bold uppercase mb-4">Submit Your Bid</h6>
          <form @submit.prevent="handleBidSubmit" class="space-y-4">
            <div class="grid grid-cols-1 sm:grid-cols-4 gap-4">
              <div class="sm:col-span-1">
                <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">Amount ($)</label>
                <input v-model="bidAmount" type="number" class="border-0 px-3 py-2 bg-white rounded text-sm shadow focus:outline-none w-full" required />
              </div>
              <div class="sm:col-span-3">
                <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">Proposal / Description</label>
                <input v-model="bidDescription" type="text" class="border-0 px-3 py-2 bg-white rounded text-sm shadow focus:outline-none w-full" placeholder="Why should they pick you?" required />
              </div>
            </div>
            <div class="flex justify-end">
              <button :disabled="isSubmitting" type="submit" class="bg-emerald-500 text-white font-bold uppercase text-xs px-6 py-2 rounded shadow hover:shadow-md outline-none focus:outline-none transition-all duration-150">
                Submit Bid
              </button>
            </div>
          </form>
        </div>
      </div>

      <!-- Bids Table -->
      <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded bg-white">
        <div class="rounded-t mb-0 px-4 py-3 border-0">
          <h3 class="font-semibold text-lg text-blueGray-700">Received Bids</h3>
        </div>
        <div class="block w-full overflow-x-auto">
          <table class="items-center w-full bg-transparent border-collapse">
            <thead>
              <tr>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">Provider ID</th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">Amount</th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100">Status</th>
                <th class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="bid in bids" :key="bid.id">
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4">{{ bid.masterId.slice(0, 8) }}...</td>
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 font-bold text-blueGray-600">${{ bid.amount }}</td>
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
                <td class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 text-right">
                  <button v-if="isClient && request.status === 'Open' && bid.status === 'Pending'" @click="handleAcceptBid(bid.id)" class="bg-blue-500 text-white font-bold uppercase text-[10px] px-3 py-1 rounded shadow hover:shadow-md outline-none focus:outline-none transition-all duration-150">
                    Accept
                  </button>
                </td>
              </tr>
              <tr v-if="bids.length === 0">
                <td colspan="4" class="text-center py-8 text-blueGray-400 italic">No bids received yet.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
