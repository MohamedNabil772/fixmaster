<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../services/api'

const router = useRouter()
const title = ref('')
const description = ref('')
const category = ref('Plumbing')
const budget = ref(0)
const isSubmitting = ref(false)

const categories = ['Plumbing', 'Electrical', 'Carpentry', 'Painting', 'Cleaning', 'Other']

const handleSubmit = async () => {
  isSubmitting.value = true
  try {
    await api.post('/api/bidding/requests', {
      title: title.value,
      description: description.value,
      category: category.value,
      budget: budget.value
    })
    alert('Request posted successfully!')
    router.push('/dashboard')
  } catch (error) {
    console.error('Failed to post request', error)
    alert('Failed to post request')
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <div class="flex flex-wrap mt-4">
    <div class="w-full lg:w-8/12 px-4 mx-auto">
      <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-blueGray-100 border-0">
        <div class="rounded-t bg-white mb-0 px-6 py-6">
          <div class="text-center flex justify-between">
            <h6 class="text-blueGray-700 text-xl font-bold">Post New Service Request</h6>
          </div>
        </div>
        <div class="flex-auto px-4 lg:px-10 py-10 pt-0">
          <form @submit.prevent="handleSubmit">
            <h6 class="text-blueGray-400 text-sm mt-3 mb-6 font-bold uppercase">
              Service Details
            </h6>
            <div class="flex flex-wrap">
              <div class="w-full px-4">
                <div class="relative w-full mb-3">
                  <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
                    Title
                  </label>
                  <input
                    v-model="title"
                    type="text"
                    placeholder="e.g. Fix leaking kitchen sink"
                    class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none focus:ring w-full ease-linear transition-all duration-150"
                    required
                  />
                </div>
              </div>
              <div class="w-full lg:w-6/12 px-4">
                <div class="relative w-full mb-3">
                  <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
                    Category
                  </label>
                  <select
                    v-model="category"
                    class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none focus:ring w-full ease-linear transition-all duration-150"
                  >
                    <option v-for="cat in categories" :key="cat" :value="cat">{{ cat }}</option>
                  </select>
                </div>
              </div>
              <div class="w-full lg:w-6/12 px-4">
                <div class="relative w-full mb-3">
                  <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
                    Estimated Budget ($)
                  </label>
                  <input
                    v-model="budget"
                    type="number"
                    class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none focus:ring w-full ease-linear transition-all duration-150"
                    required
                  />
                </div>
              </div>
              <div class="w-full px-4">
                <div class="relative w-full mb-3">
                  <label class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
                    Description
                  </label>
                  <textarea
                    v-model="description"
                    rows="4"
                    placeholder="Describe the issue in detail..."
                    class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none focus:ring w-full ease-linear transition-all duration-150"
                    required
                  ></textarea>
                </div>
              </div>
            </div>

            <div class="flex justify-end px-4 mt-6">
              <button
                type="submit"
                :disabled="isSubmitting"
                class="bg-blue-500 text-white active:bg-blue-600 font-bold uppercase text-xs px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150 disabled:opacity-50"
              >
                {{ isSubmitting ? 'Posting...' : 'Post Request' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
