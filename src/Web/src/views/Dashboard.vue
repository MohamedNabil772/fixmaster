<script setup lang="ts">
import { onMounted, computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useBiddingStore } from '../stores/bidding'
import { useAuthStore } from '../stores/auth'
import AppCard from '../components/common/AppCard.vue'
import AppButton from '../components/common/AppButton.vue'
import AdminDashboard from './admin/Dashboard.vue'

const router = useRouter()
const biddingStore = useBiddingStore()
const authStore = useAuthStore()

const isClient = computed(() => authStore.user?.role === 'Client')
const isAdmin = computed(() => authStore.user?.role === 'Admin' || authStore.user?.role === 'SuperAdmin')

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
    <!-- Admin/SuperAdmin Dashboard (Graphs Only) -->
    <template v-if="isAdmin">
      <AdminDashboard />
    </template>

    <!-- Client Dashboard -->
    <template v-else-if="isClient">
      <div class="flex justify-between items-center">
        <h2 class="text-2xl font-bold text-primary">Your Service Requests</h2>
        <AppButton variant="primary" @click="router.push('/post-request')">New Request</AppButton>
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
              'text-yellow-600': request.status === 'Open',
              'text-green-600': request.status === 'InProgress',
              'text-blue-600': request.status === 'Completed'
            }">
              {{ request.status }}
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
              'text-yellow-600': bid.status === 'Pending',
              'text-green-600': bid.status === 'Accepted',
              'text-red-600': bid.status === 'Rejected'
            }">
              {{ bid.status }}
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
