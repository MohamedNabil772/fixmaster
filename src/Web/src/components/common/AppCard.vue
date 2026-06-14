<script setup lang="ts">
interface Props {
  title?: string;
  noPadding?: boolean;
}

defineProps<Props>();
</script>

<template>
  <div class="bg-white rounded-lg shadow-sm border border-gray-100 overflow-hidden">
    <div v-if="title || $slots.header" class="px-6 py-4 border-b border-gray-100">
      <slot name="header">
        <h3 v-if="title" class="text-lg font-semibold text-primary">{{ title }}</h3>
      </slot>
    </div>
    <div :class="{ 'px-6 py-4': !noPadding }">
      <slot />
    </div>
    <div v-if="$slots.footer" class="px-6 py-4 bg-gray-50 border-t border-gray-100">
      <slot name="footer" />
    </div>
  </div>
</template>
