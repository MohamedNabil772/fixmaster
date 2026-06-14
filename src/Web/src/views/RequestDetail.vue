<script setup lang="ts">
import { onMounted, ref, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useBiddingStore } from '../stores/bidding'
import { useAuthStore } from '../stores/auth'
import AppCard from '../components/common/AppCard.vue'
import AppButton from '../components/common/AppButton.vue'
import FeedbackForm from '../components/FeedbackForm.vue'

const route = useRoute()
const router = useRouter()
const biddingStore = useBiddingStore()
const authStore = useAuthStore()

const requestId = route.params.id as string
const showFeedbackForm = ref(false)

const request = computed(() => 
  biddingStore.serviceRequests.find(r => r.id === requestId)
)

const isClient = computed(() => authStore.user?.role === 'Client')

onMounted(async () => {
  await biddingStore.fetchRequestDetails(requestId)
  await biddingStore.fetchBids(requestId)
})

async function handleAcceptBid(bidId: string) {
  try {
    await biddingStore.acceptBid(bidId)
    await biddingStore.fetchRequestDetails(requestId)
    await biddingStore.fetchBids(requestId)
  } catch (error) {
    alert('Failed to accept bid')
  }
}

function handleFeedbackSubmitted() {
  showFeedbackForm.value = false
  biddingStore.fetchRequestDetails(requestId)
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center space-x-4">
      <button @click="router.back()" class="text-secondary hover:text-primary">
        &larr; Back
      </button>
      <h2 class="text-2xl font-bold text-primary">Request Details</h2>
    </div>

    <div v-if="biddingStore.isLoading && !request" class="flex justify-center py-12">
      <p class="text-secondary">Loading details...</p>
    </div>

    <div v-else-if="request" class="grid gap-6 lg:grid-cols-3">
      <div class="lg:col-span-2 space-y-6">
        <AppCard :title="request.title">
          <p class="text-secondary mb-6">{{ request.description }}</p>
          
          <div class="flex flex-wrap gap-4 text-sm">
            <div class="px-3 py-1 bg-gray-100 rounded-full">
              <span class="font-semibold">Budget:</span> ${{ request.budget }}
            </div>
            <div class="px-3 py-1 bg-gray-100 rounded-full">
              <span class="font-semibold">Status:</span> 
              <span :class="{
                'text-yellow-600': request.status === 'pending',
                'text-green-600': request.status === 'active',
                'text-blue-600': request.status === 'completed'
              }" class="ml-1 uppercase text-xs">
                {{ request.status }}
              </span>
            </div>
            <div class="px-3 py-1 bg-gray-100 rounded-full text-secondary">
              Created: {{ new Date(request.createdAt).toLocaleDateString() }}
            </div>
          </div>
        </AppCard>

        <!-- Bids Section -->
        <div class="space-y-4">
          <h3 class="text-xl font-bold text-primary">Bids</h3>
          
          <div v-if="!request.bids || request.bids.length === 0" class="bg-white p-8 rounded-lg border-2 border-dashed border-gray-200 text-center">
            <p class="text-secondary">No bids received yet.</p>
          </div>

          <div v-else class="grid gap-4">
            <AppCard v-for="bid in request.bids" :key="bid.id">
              <div class="flex justify-between items-start">
                <div>
                  <h4 class="font-bold text-primary">{{ bid.masterName }}</h4>
                  <p class="text-sm text-secondary mt-1">{{ bid.description }}</p>
                </div>
                <div class="text-right">
                  <div class="text-lg font-bold text-primary">${{ bid.amount }}</div>
                  <div class="text-xs text-secondary">{{ new Date(bid.createdAt).toLocaleDateString() }}</div>
                </div>
              </div>

              <template v-if="isClient && request.status === 'pending'" #footer>
                <div class="flex justify-end">
                  <AppButton @click="handleAcceptBid(bid.id)" size="sm">
                    Accept Bid
                  </AppButton>
                </div>
              </template>

              <div v-else-if="bid.status === 'accepted'" class="mt-4 pt-4 border-t border-gray-100">
                <span class="text-sm font-bold text-green-600 uppercase">Accepted Bid</span>
              </div>
            </AppCard>
          </div>
        </div>
      </div>

      <div class="space-y-6">
        <AppCard title="Actions">
          <div class="space-y-3">
            <template v-if="isClient && request.status === 'completed'">
              <AppButton @click="showFeedbackForm = true" class="w-full">
                Leave Feedback
              </AppButton>
            </template>
            <p v-else class="text-sm text-secondary italic">
              No actions available at this time.
            </p>
          </div>
        </AppCard>

        <FeedbackForm 
          v-if="showFeedbackForm" 
          :request-id="requestId"
          @submitted="handleFeedbackSubmitted"
          @cancel="showFeedbackForm = false"
        />
      </div>
    </div>
  </div>
</template>
