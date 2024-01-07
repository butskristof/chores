<template>
  <AppHeader />

  <div class="test-content">
    <div
      v-for="i in new Array(500).keys()"
      :key="i"
    >
      hey {{ i }}
    </div>
  </div>

  <RouterView />
</template>

<script setup>
import AppHeader from '@/components/header/AppHeader.vue';
</script>
