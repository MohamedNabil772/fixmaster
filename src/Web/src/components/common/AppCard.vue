<script setup lang="ts">
interface Props {
  title?: string;
  noPadding?: boolean;
}

defineProps<Props>();
</script>

<template>
  <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded bg-white border-0">
    <div v-if="title || $slots.header" class="rounded-t mb-0 px-4 py-3 border-0 bg-blueGray-50">
      <div class="flex flex-wrap items-center">
        <div class="relative w-full px-4 max-w-full flex-grow flex-1">
          <slot name="header">
            <h3 v-if="title" class="font-semibold text-lg text-blueGray-700">
              {{ title }}
            </h3>
          </slot>
        </div>
      </div>
    </div>
    <div class="flex-auto" :class="{ 'p-6': !noPadding }">
      <slot />
    </div>
    <div v-if="$slots.footer" class="px-6 py-4 bg-blueGray-50 border-t border-solid border-blueGray-100 rounded-b">
      <slot name="footer" />
    </div>
  </div>
</template>
