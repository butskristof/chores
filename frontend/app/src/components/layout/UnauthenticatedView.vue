<template>
  <div class="unauthenticated-view">
    <h1>You are not logged in</h1>
    <p>Please log in to access the application.</p>
    <div class="actions">
      <a :href="`/bff/login?returnUrl=${returnUrl}`">
        <PrimeButton
          label="Log in"
          icon="pi pi-sign-in"
        />
      </a>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import PrimeButton from 'primevue/button';

const returnUrl = computed(() => window.location.origin);
</script>

<style scoped lang="scss">
@import '@/styles/utilities';

.unauthenticated-view {
  @include info-page;
}
</style>
