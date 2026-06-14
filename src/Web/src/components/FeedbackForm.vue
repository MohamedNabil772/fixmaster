<script setup lang="ts">
import { ref } from 'vue'
import { useBiddingStore } from '../stores/bidding'
import AppCard from './common/AppCard.vue'
import AppButton from './common/AppButton.vue'
import AppInput from './common/AppInput.vue'

interface Props {
  requestId: string;
}

const props = defineProps<Props>();
const emit = defineEmits(['submitted', 'cancel']);

const biddingStore = useBiddingStore()
const rating = ref(5)
const comment = ref('')
const isSubmitting = ref(false)

async function handleSubmit() {
  isSubmitting.value = true
  try {
    await biddingStore.submitFeedback({
      requestId: props.requestId,
      rating: rating.value,
      comment: comment.value
    })
    emit('submitted')
  } catch (error) {
    alert('Failed to submit feedback')
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <AppCard title="Rate the Service">
    <form @submit.prevent="handleSubmit" class="space-y-4">
      <div>
        <label class="block text-sm font-medium text-secondary mb-1">Rating (1-5)</label>
        <div class="flex items-center space-x-2">
          <button 
            v-for="i in 5" 
            :key="i"
            type="button"
            @click="rating = i"
            class="text-2xl focus:outline-none"
            :class="i <= rating ? 'text-yellow-400' : 'text-gray-300'"
          >
            ★
          </button>
        </div>
      </div>

      <div>
        <label class="block text-sm font-medium text-secondary mb-1">Comment</label>
        <textarea
          v-model="comment"
          rows="3"
          class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent"
          placeholder="How was the service?"
          required
        ></textarea>
      </div>

      <div class="flex space-x-3">
        <AppButton 
          type="submit" 
          class="flex-1" 
          :disabled="isSubmitting"
        >
          {{ isSubmitting ? 'Submitting...' : 'Submit' }}
        </AppButton>
        <AppButton 
          variant="outline" 
          @click="emit('cancel')"
          :disabled="isSubmitting"
        >
          Cancel
        </AppButton>
      </div>
    </form>
  </AppCard>
</template>
