<template>
  <div class="app-layout">
    <AppHeader class="app-header" />
    <div class="app-body">
      <main class="page">
        <RouterView />
      </main>
      <AppFooter class="app-footer" />
    </div>
  </div>
</template>

<script setup>
import { RouterView } from 'vue-router';
import AppHeader from '@/components/layout/AppHeader.vue';
import AppFooter from '@/components/layout/AppFooter.vue';
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.app-layout {
  min-height: 100vh;
  background-color: var(--surface-ground);
  @include flex-column;
}

.app-header {
  flex-shrink: 0;
}

.app-body {
  flex-grow: 1;
  @include flex-column;
  overflow-y: auto;

  .page {
    flex-grow: 1;
    padding-inline: var(--default-padding);
  }

  .app-footer {
    flex-grow: 0;
    flex-shrink: 0;
  }
}
</style>
