import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '../services/api'

export interface ServiceRequest {
  id: string;
  title: string;
  description: string;
  status: 'pending' | 'active' | 'completed' | 'cancelled';
  budget: number;
  createdAt: string;
  bids?: Bid[];
}

export interface Bid {
  id: string;
  requestId: string;
  masterId: string;
  masterName: string;
  amount: number;
  description: string;
  status: 'pending' | 'accepted' | 'rejected';
  createdAt: string;
}

export interface BidData {
  requestId: string;
  amount: number;
  description: string;
}

export interface FeedbackData {
  requestId: string;
  rating: number;
  comment: string;
}

export const useBiddingStore = defineStore('bidding', () => {
  const serviceRequests = ref<ServiceRequest[]>([])
  const myBids = ref<Bid[]>([])
  const isLoading = ref(false)

  async function fetchServiceRequests() {
    isLoading.value = true
    try {
      const response = await api.get<ServiceRequest[]>('/api/requests')
      serviceRequests.value = response.data
    } catch (error) {
      console.error('Failed to fetch service requests:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function fetchRequestDetails(requestId: string) {
    isLoading.value = true
    try {
      const response = await api.get<ServiceRequest>(`/api/requests/${requestId}`)
      const index = serviceRequests.value.findIndex(r => r.id === requestId)
      if (index !== -1) {
        serviceRequests.value[index] = response.data
      } else {
        serviceRequests.value.push(response.data)
      }
      return response.data
    } catch (error) {
      console.error('Failed to fetch request details:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function fetchBids(requestId: string) {
    isLoading.value = true
    try {
      const response = await api.get<Bid[]>(`/api/bids/request/${requestId}`)
      const request = serviceRequests.value.find(r => r.id === requestId)
      if (request) {
        request.bids = response.data
      }
      return response.data
    } catch (error) {
      console.error('Failed to fetch bids:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function submitBid(bidData: BidData) {
    isLoading.value = true
    try {
      const response = await api.post('/api/bids', bidData)
      return response.data
    } catch (error) {
      console.error('Failed to submit bid:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function acceptBid(requestId: string, bidId: string) {
    isLoading.value = true
    try {
      const response = await api.post('/api/bids/select-master', { requestId, bidId })
      return response.data
    } catch (error) {
      console.error('Failed to accept bid:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function fetchMyBids() {
    isLoading.value = true
    try {
      const response = await api.get<Bid[]>('/api/bidding/my-bids')
      myBids.value = response.data
    } catch (error) {
      console.error('Failed to fetch my bids:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function submitFeedback(feedbackData: FeedbackData) {
    isLoading.value = true
    try {
      const response = await api.post('/api/feedback', feedbackData)
      return response.data
    } catch (error) {
      console.error('Failed to submit feedback:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  return {
    serviceRequests,
    myBids,
    isLoading,
    fetchServiceRequests,
    fetchRequestDetails,
    fetchBids,
    submitBid,
    acceptBid,
    fetchMyBids,
    submitFeedback
  }
})
