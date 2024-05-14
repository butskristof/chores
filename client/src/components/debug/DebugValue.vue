<template>
  <pre>{{ JSON.stringify(value, null, 2) }}</pre>
</template>

<script setup>
defineProps({
  value: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

pre {
  @include pre-overflow;
}
</style>
