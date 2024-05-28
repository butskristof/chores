<template>
  <div class="page-not-found">
    <h1>Page not found</h1>
    <p>Sorry, we couldn't find the page you requested.</p>
    <div class="actions">
      <router-link :to="{ name: routes.home.name }">
        <PrimeButton
          label="Go to dashboard"
          icon="pi pi-home"
        />
      </router-link>
    </div>
  </div>
</template>

<script setup>
import { routes } from '@/router/routes.js';
import PrimeButton from 'primevue/button';
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.page-not-found {
  @include info-page;
}
</style>
