<template>
  <main>
    <div
      v-if="isLoading"
      class="loading"
    >
      <PrimeProgressSpinner />
      <div>Logging in...</div>
    </div>
    <AuthenticatedView v-else-if="isAuthenticated" />
    <UnauthenticatedView v-else />
  </main>
</template>

<script setup>
import { useBffUser } from '@/composables/queries/auth.js';
import AuthenticatedView from '@/components/layout/AuthenticatedView.vue';
import UnauthenticatedView from '@/components/layout/UnauthenticatedView.vue';
import PrimeProgressSpinner from 'primevue/progressspinner';

const { isLoading, isAuthenticated } = useBffUser();
</script>

<style scoped lang="scss">
@import '@/styles/utilities';

main {
  // TODO transition
}

.loading {
  @include info-page;
  gap: 1rem;
}
</style>
