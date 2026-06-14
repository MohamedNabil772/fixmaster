<script setup lang="ts">
import { ref, computed } from 'vue'

interface Props {
  modelValue: string | number;
  label?: string;
  type?: string;
  placeholder?: string;
  error?: string;
  required?: boolean;
  id?: string;
}

const props = defineProps<Props>();
const emit = defineEmits(['update:modelValue']);

const showPassword = ref(false)

const inputType = computed(() => {
  if (props.type === 'password') {
    return showPassword.value ? 'text' : 'password'
  }
  return props.type || 'text'
})

const togglePassword = () => {
  showPassword.value = !showPassword.value
}
</script>

<template>
  <div class="relative w-full mb-3">
    <label v-if="label" :for="id" class="block uppercase text-blueGray-600 text-xs font-bold mb-2">
      {{ label }} <span v-if="required" class="text-red-500">*</span>
    </label>
    <div class="relative">
      <input
        :id="id"
        :type="inputType"
        :value="modelValue"
        @input="emit('update:modelValue', ($event.target as HTMLInputElement).value)"
        :placeholder="placeholder"
        class="border-0 px-3 py-3 placeholder-blueGray-300 text-blueGray-600 bg-white rounded text-sm shadow focus:outline-none focus:ring w-full ease-linear transition-all duration-150"
        :class="{
          'ring-1 ring-red-500': error
        }"
        :required="required"
      />
      <button 
        v-if="type === 'password'"
        type="button"
        @click="togglePassword"
        class="absolute right-3 top-1/2 -translate-y-1/2 text-blueGray-400 hover:text-blueGray-600 focus:outline-none"
      >
        <i class="fas" :class="showPassword ? 'fa-eye-slash' : 'fa-eye'"></i>
      </button>
    </div>
    <p v-if="error" class="text-xs text-red-500 mt-1 italic">{{ error }}</p>
  </div>
</template>
