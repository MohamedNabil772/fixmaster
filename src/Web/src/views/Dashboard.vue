<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useBiddingStore } from '../stores/bidding'
import { useAuthStore } from '../stores/auth'
import AppCard from '../components/common/AppCard.vue'
import AppButton from '../components/common/AppButton.vue'

const router = useRouter()
const biddingStore = useBiddingStore()
const authStore = useAuthStore()

const isClient = computed(() => authStore.user?.role === 'Client')
const isAdmin = computed(() => authStore.user?.role === 'Admin')

onMounted(() => {
  if (isClient.value) {
    biddingStore.fetchServiceRequests()
  } else if (!isAdmin.value) {
    biddingStore.fetchMyBids()
  }
})

function navigateToRequest(id: string) {
  router.push(`/requests/${id}`)
}
</script>

<template>
  <div class="space-y-6">
    <!-- Client Dashboard -->
    <template v-if="isClient">
      <div class="flex justify-between items-center">
        <h2 class="text-2xl font-bold text-primary">Your Service Requests</h2>
        <AppButton variant="primary">New Request</AppButton>
      </div>

      <div v-if="biddingStore.isLoading && biddingStore.serviceRequests.length === 0" class="flex justify-center py-12">
        <p class="text-secondary">Loading requests...</p>
      </div>

      <div v-else-if="biddingStore.serviceRequests.length === 0" class="text-center py-12 bg-white rounded-lg border-2 border-dashed border-gray-200">
        <p class="text-secondary">No service requests found.</p>
      </div>

      <div v-else class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <AppCard 
          v-for="request in biddingStore.serviceRequests" 
          :key="request.id"
          :title="request.title"
        >
          <p class="text-sm text-secondary line-clamp-2 mb-4">
            {{ request.description }}
          </p>
          
          <div class="flex justify-between items-center text-sm font-medium">
            <span :class="{
              'text-yellow-600': request.status === 'pending',
              'text-green-600': request.status === 'active',
              'text-blue-600': request.status === 'completed'
            }">
              {{ request.status.charAt(0).toUpperCase() + request.status.slice(1) }}
            </span>
            <span class="text-primary">${{ request.budget }}</span>
          </div>

          <template #footer>
            <div class="flex justify-end">
              <button 
                @click="navigateToRequest(request.id)"
                class="text-accent text-sm font-semibold hover:underline"
              >
                View Details
              </button>
            </div>
          </template>
        </AppCard>
      </div>
    </template>

    <!-- Admin Dashboard -->
    <template v-else-if="isAdmin">
      <div class="flex flex-col items-center justify-center py-24 bg-white rounded-lg shadow-md border border-gray-100">
        <div class="p-4 bg-emerald-100 rounded-full mb-6">
          <i class="fas fa-user-shield text-4xl text-emerald-600"></i>
        </div>
        <h2 class="text-3xl font-bold text-gray-800 mb-2">Admin Control Center</h2>
        <p class="text-gray-500 mb-8 max-w-md text-center">
          Welcome to the FixMaster administrative dashboard. From here you can manage all system activities, monitor bids, and oversee service requests.
        </p>
        <div class="flex gap-4">
          <AppButton variant="primary" @click="router.push('/admin/bids')">
            Manage Bids
          </AppButton>
          <AppButton variant="secondary" @click="router.push('/admin/services')">
            System Overview
          </AppButton>
        </div>
      </div>
    </template>

    <!-- Master Dashboard -->
    <template v-else>
      <div class="flex justify-between items-center">
        <h2 class="text-2xl font-bold text-primary">Your Bids</h2>
        <AppButton variant="primary" @click="router.push('/browse-requests')">Browse Requests</AppButton>
      </div>

      <div v-if="biddingStore.isLoading && biddingStore.myBids.length === 0" class="flex justify-center py-12">
        <p class="text-secondary">Loading bids...</p>
      </div>

      <div v-else-if="biddingStore.myBids.length === 0" class="text-center py-12 bg-white rounded-lg border-2 border-dashed border-gray-200">
        <p class="text-secondary">You haven't placed any bids yet.</p>
      </div>

      <div v-else class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <AppCard 
          v-for="bid in biddingStore.myBids" 
          :key="bid.id"
          :title="`Bid for Request #${bid.requestId.slice(0, 8)}`"
        >
          <p class="text-sm text-secondary line-clamp-2 mb-4">
            {{ bid.description }}
          </p>
          
          <div class="flex justify-between items-center text-sm font-medium">
            <span :class="{
              'text-yellow-600': bid.status === 'pending',
              'text-green-600': bid.status === 'accepted',
              'text-red-600': bid.status === 'rejected'
            }">
              {{ bid.status.charAt(0).toUpperCase() + bid.status.slice(1) }}
            </span>
            <span class="text-primary">${{ bid.amount }}</span>
          </div>

          <template #footer>
            <div class="flex justify-end">
              <button 
                @click="navigateToRequest(bid.requestId)"
                class="text-accent text-sm font-semibold hover:underline"
              >
                View Request
              </button>
            </div>
          </template>
        </AppCard>
      </div>
    </template>
  </div>
</template>
