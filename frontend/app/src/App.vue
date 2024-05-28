<template>
  <div class="app-layout">
    <AppHeader class="app-header" />
    <div class="app-body">
      <AppMain class="page" />
      <AppFooter class="app-footer" />
    </div>
  </div>
</template>

<script setup>
import AppHeader from '@/components/layout/AppHeader.vue';
import AppFooter from '@/components/layout/AppFooter.vue';
import AppMain from '@/components/layout/AppMain.vue';
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.app-layout {
  min-height: 100vh;
  background-color: var(--ground-background);
  @include flex-column;
}

.app-header {
  flex-shrink: 0;
}

.app-body {
  flex-grow: 1;
  @include flex-column;

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
