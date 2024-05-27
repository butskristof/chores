<template>
  <PrimeSkeleton
    height="10rem"
    width="100%"
    class="chore-list-item"
  />
</template>

<script setup>
import PrimeSkeleton from 'primevue/skeleton';
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.chore-list-item {
  margin-bottom: 1rem;
}
</style>
