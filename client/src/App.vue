<template>
  <header>
    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink :to="{ name: routes.tags.name }">Tags</RouterLink>
    </nav>
    <div class="right">
      <ColorSchemePicker />
      <HeaderAuth />
    </div>
  </header>

  <RouterView />
</template>

<script setup>
import HeaderAuth from '@/components/header/HeaderAuth.vue';
import { routes } from '@/router/routes';
import ColorSchemePicker from '@/components/header/ColorSchemePicker.vue';
</script>

<style scoped lang="scss">
@import '@/styles/utilities.scss';

header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;

  margin-bottom: 1rem;

  nav {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
  }

  .right {
    @include flex-row;
    align-items: center;
    gap: var(--size-2);
  }
}
</style>
