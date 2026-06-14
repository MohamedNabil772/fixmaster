<script setup lang="ts">
import { onMounted, ref, computed, watch } from 'vue'
import { useBiddingStore } from '../../stores/bidding'
import { Bar, Line, Pie } from 'vue-chartjs'
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  ArcElement
} from 'chart.js'

ChartJS.register(
  Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, PointElement, LineElement, ArcElement
)

const biddingStore = useBiddingStore()
const filterType = ref('year')
const serviceFilter = ref('')

const fetchData = () => {
  biddingStore.fetchAdminStats(filterType.value, serviceFilter.value || undefined)
}

onMounted(() => {
  fetchData()
})

watch([filterType, serviceFilter], () => {
  fetchData()
})

// Chart Configurations
const bidsChartData = computed(() => ({
  labels: biddingStore.adminStats?.bidsByService.map(d => d.label) || [],
  datasets: [{
    label: 'Number of Bids',
    backgroundColor: '#3b82f6',
    data: biddingStore.adminStats?.bidsByService.map(d => d.value) || []
  }]
}))

const requestsChartData = computed(() => ({
  labels: biddingStore.adminStats?.requestsByService.map(d => d.label) || [],
  datasets: [{
    label: 'User Requests',
    backgroundColor: '#10b981',
    data: biddingStore.adminStats?.requestsByService.map(d => d.value) || []
  }]
}))

const mastersChartData = computed(() => ({
  labels: biddingStore.adminStats?.mastersByService.map(d => d.label) || [],
  datasets: [{
    label: 'Masters Count',
    backgroundColor: '#f59e0b',
    data: biddingStore.adminStats?.mastersByService.map(d => d.value) || []
  }]
}))

const timelineChartData = computed(() => ({
  labels: biddingStore.adminStats?.timelineData.map(d => d.label) || [],
  datasets: [{
    label: 'Activity Over Time',
    borderColor: '#6366f1',
    tension: 0.4,
    data: biddingStore.adminStats?.timelineData.map(d => d.value) || []
  }]
}))

const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: { display: false }
  }
}
</script>

<template>
  <div class="space-y-6 mt-4">
    <!-- Filters -->
    <div class="bg-white p-4 rounded-lg shadow-sm flex flex-wrap gap-6 items-center border border-gray-100">
      <div class="flex items-center gap-3">
        <span class="text-xs font-bold text-gray-400 uppercase tracking-wider">Time Range:</span>
        <div class="flex bg-gray-100 p-1 rounded-md">
          <button 
            v-for="t in ['day', 'month', 'year']" 
            :key="t"
            @click="filterType = t"
            class="px-3 py-1 text-xs font-bold rounded capitalize transition-all"
            :class="filterType === t ? 'bg-white text-primary shadow-sm' : 'text-gray-500 hover:text-gray-700'"
          >
            {{ t }}
          </button>
        </div>
      </div>
      
      <div class="flex items-center gap-3">
        <span class="text-xs font-bold text-gray-400 uppercase tracking-wider">Service Category:</span>
        <select 
          v-model="serviceFilter"
          class="text-xs font-bold border-none bg-gray-100 rounded-md focus:ring-primary py-2 px-3"
        >
          <option value="">All Services</option>
          <option value="Plumbing">Plumbing</option>
          <option value="Electrical">Electrical</option>
          <option value="Carpentry">Carpentry</option>
          <option value="Painting">Painting</option>
        </select>
      </div>
    </div>

    <!-- Stats Cards -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <div class="bg-white p-6 rounded-lg shadow-sm border-l-4 border-blue-500">
        <p class="text-xs font-bold text-gray-400 uppercase">Total Users</p>
        <p class="text-2xl font-black text-gray-700">{{ biddingStore.adminStats?.totalUsers || 0 }}</p>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-sm border-l-4 border-purple-500">
        <p class="text-xs font-bold text-gray-400 uppercase">Total Bids</p>
        <p class="text-2xl font-black text-gray-700">{{ biddingStore.adminStats?.totalBids || 0 }}</p>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-sm border-l-4 border-yellow-500">
        <p class="text-xs font-bold text-gray-400 uppercase">Active Masters</p>
        <p class="text-2xl font-black text-gray-700">{{ biddingStore.adminStats?.totalMasters || 0 }}</p>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-sm border-l-4 border-emerald-500">
        <p class="text-xs font-bold text-gray-400 uppercase">Total Earnings</p>
        <p class="text-2xl font-black text-gray-700">${{ biddingStore.adminStats?.totalEarnings.toLocaleString() || 0 }}</p>
      </div>
    </div>

    <!-- Charts Row 1 -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <div class="bg-white p-6 rounded-lg shadow-sm border border-gray-100">
        <h4 class="text-sm font-bold text-gray-700 mb-6 uppercase tracking-wide">Bids per Service</h4>
        <div class="h-64">
          <Bar :data="bidsChartData" :options="chartOptions" />
        </div>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-sm border border-gray-100">
        <h4 class="text-sm font-bold text-gray-700 mb-6 uppercase tracking-wide">Service Demand (Requests)</h4>
        <div class="h-64">
          <Bar :data="requestsChartData" :options="chartOptions" />
        </div>
      </div>
    </div>

    <!-- Charts Row 2 -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <div class="bg-white p-6 rounded-lg shadow-sm border border-gray-100">
        <h4 class="text-sm font-bold text-gray-700 mb-6 uppercase tracking-wide">Growth Trend</h4>
        <div class="h-64">
          <Line :data="timelineChartData" :options="chartOptions" />
        </div>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-sm border border-gray-100">
        <h4 class="text-sm font-bold text-gray-700 mb-6 uppercase tracking-wide">Masters Distribution</h4>
        <div class="h-64">
          <Pie :data="mastersChartData" :options="{ ...chartOptions, plugins: { legend: { display: true, position: 'right' } } }" />
        </div>
      </div>
    </div>
  </div>
</template>
