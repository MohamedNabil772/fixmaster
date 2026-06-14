<script setup lang="ts">
interface Props {
  variant?: 'primary' | 'secondary' | 'outline' | 'danger';
  type?: 'button' | 'submit' | 'reset';
  disabled?: boolean;
}

withDefaults(defineProps<Props>(), {
  variant: 'primary',
  type: 'button',
  disabled: false,
});
</script>

<template>
  <button
    :type="type"
    :disabled="disabled"
    class="px-4 py-2 rounded-md font-medium transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed"
    :class="{
      'bg-primary text-white hover:bg-opacity-90 focus:ring-primary': variant === 'primary',
      'bg-secondary text-white hover:bg-opacity-90 focus:ring-secondary': variant === 'secondary',
      'border-2 border-primary text-primary hover:bg-primary hover:text-white focus:ring-primary': variant === 'outline',
      'bg-accent text-white hover:bg-opacity-90 focus:ring-accent': variant === 'danger'
    }"
  >
    <slot />
  </button>
</template>
