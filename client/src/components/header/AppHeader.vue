<template>
  <header>
    <div class="brand">Chores</div>
    <div class="actions">
      <AuthenticatedHeaderActions v-if="authStore.isAuthenticated" />
      <UnauthenticatedHeaderActions v-else />
    </div>
  </header>
</template>

<script setup>
import { useAuthStore } from '@/stores/auth.js';
import AuthenticatedHeaderActions from '@/components/header/AuthenticatedHeaderActions.vue';
import UnauthenticatedHeaderActions from '@/components/header/UnauthenticatedHeaderActions.vue';

const authStore = useAuthStore();
</script>

<style scoped lang="scss">
@import '@/styles/utilities';

header {
  position: sticky;
  top: 0;

  margin-bottom: var(--size-2);

  background-color: var(--surface-1);
  box-shadow: var(--shadow-1);

  @include flex-row-justify-between;
  align-items: center;
  padding-inline: var(--size-3);
  padding-block: var(--size-2);

  .brand {
    font-size: var(--font-size-4);
    font-weight: var(--font-weight-6);
  }
}
</style>
