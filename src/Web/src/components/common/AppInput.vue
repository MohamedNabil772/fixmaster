<script setup lang="ts">
interface Props {
  modelValue: string | number;
  label?: string;
  type?: string;
  placeholder?: string;
  error?: string;
  required?: boolean;
  id?: string;
}

defineProps<Props>();
defineEmits(['update:modelValue']);
</script>

<template>
  <div class="flex flex-col gap-1 w-full">
    <label v-if="label" :for="id" class="text-sm font-medium text-secondary">
      {{ label }} <span v-if="required" class="text-accent">*</span>
    </label>
    <input
      :id="id"
      :type="type || 'text'"
      :value="modelValue"
      @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
      :placeholder="placeholder"
      class="px-3 py-2 border rounded-md focus:outline-none focus:ring-2 transition-all duration-200"
      :class="{
        'border-gray-300 focus:ring-primary focus:border-primary': !error,
        'border-accent focus:ring-accent focus:border-accent': error
      }"
    />
    <p v-if="error" class="text-xs text-accent mt-1">{{ error }}</p>
  </div>
</template>
