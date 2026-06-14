<script setup lang="ts">
interface Props {
  variant?: 'primary' | 'secondary' | 'outline' | 'danger' | 'admin' | 'success';
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
    class="px-4 py-2 rounded font-bold uppercase text-xs shadow hover:shadow-md outline-none focus:outline-none ease-linear transition-all duration-150 disabled:opacity-50 disabled:cursor-not-allowed"
    :class="{
      'bg-blue-500 text-white active:bg-blue-600': variant === 'primary' || variant === 'admin',
      'bg-blueGray-700 text-white active:bg-blueGray-600': variant === 'secondary',
      'bg-transparent border border-solid border-blue-500 text-blue-500 hover:bg-blue-500 hover:text-white active:bg-blue-600': variant === 'outline',
      'bg-red-500 text-white active:bg-red-600': variant === 'danger',
      'bg-emerald-500 text-white active:bg-emerald-600': variant === 'success'
    }"
  >
    <slot />
  </button>
</template>
