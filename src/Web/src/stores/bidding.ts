import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '../services/api'

export interface ServiceRequest {
  id: string;
  title: string;
  description: string;
  category: string;
  status: 'Open' | 'BiddingClosed' | 'InProgress' | 'Completed' | 'Cancelled';
  budget: number;
  createdAt: string;
  bids?: Bid[];
}

export interface Bid {
  id: string;
  requestId: string;
  masterId: string;
  amount: number;
  description: string;
  status: 'Pending' | 'Accepted' | 'Rejected';
  createdAt: string;
}

export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface ChartDataPoint {
  label: string;
  value: number;
}

export interface AdminStats {
  totalUsers: number;
  totalBids: number;
  totalMasters: number;
  totalEarnings: number;
  bidsByService: ChartDataPoint[];
  requestsByService: ChartDataPoint[];
  mastersByService: ChartDataPoint[];
  timelineData: ChartDataPoint[];
}

export const useBiddingStore = defineStore('bidding', () => {
  const serviceRequests = ref<ServiceRequest[]>([])
  const allBids = ref<PaginatedList<Bid> | null>(null)
  const myBids = ref<Bid[]>([])
  const adminStats = ref<AdminStats | null>(null)
  const isLoading = ref(false)

  async function fetchServiceRequests() {
    isLoading.value = true
    try {
      const response = await api.get<ServiceRequest[]>('/api/bidding/requests')
      serviceRequests.value = response.data
    } catch (error) {
      console.error('Failed to fetch service requests:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function fetchBids(requestId: string) {
    isLoading.value = true
    try {
      const response = await api.get<Bid[]>(`/api/bidding/bids/request/${requestId}`)
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

  async function fetchAllBids(pageNumber = 1, pageSize = 10, status?: string) {
    isLoading.value = true
    try {
      const url = status 
        ? `/api/bidding/bids?pageNumber=${pageNumber}&pageSize=${pageSize}&status=${status}` 
        : `/api/bidding/bids?pageNumber=${pageNumber}&pageSize=${pageSize}`
      const response = await api.get<PaginatedList<Bid>>(url)
      allBids.value = response.data
    } catch (error) {
      console.error('Failed to fetch all bids:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function fetchAdminStats(filterType = 'year', service?: string) {
    isLoading.value = true
    try {
      const url = service 
        ? `/api/bidding/statistics/admin?filterType=${filterType}&service=${service}`
        : `/api/bidding/statistics/admin?filterType=${filterType}`
      const response = await api.get<AdminStats>(url)
      adminStats.value = response.data
    } catch (error) {
      console.error('Failed to fetch admin stats:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  async function submitBid(bidData: { requestId: string; amount: number; description: string }) {
    isLoading.value = true
    try {
      const response = await api.post('/api/bidding/bids', bidData)
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
      const response = await api.post('/api/bidding/bids/select-master', { requestId, bidId })
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
      const response = await api.get<Bid[]>('/api/bidding/bids/my-bids')
      myBids.value = response.data
    } catch (error) {
      console.error('Failed to fetch my bids:', error)
      throw error
    } finally {
      isLoading.value = false
    }
  }

  return {
    serviceRequests,
    allBids,
    myBids,
    adminStats,
    isLoading,
    fetchServiceRequests,
    fetchBids,
    fetchAllBids,
    fetchAdminStats,
    submitBid,
    acceptBid,
    fetchMyBids
  }
})
