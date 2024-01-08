<template>
  <AppHeader />
  <div v-show="false">
    <RouterView />
  </div>
</template>

<script setup>
import AppHeader from '@/components/header/AppHeader.vue';
</script>
