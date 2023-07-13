<template>
  <AppHeader />

  <div class="app-body">
    <main>
      <RouterView />
    </main>

    <AppFooter />
  </div>
</template>

<script setup>
import { RouterView } from 'vue-router';
import AppHeader from '@/components/app/AppHeader.vue';
import AppFooter from '@/components/app/AppFooter.vue';
</script>
