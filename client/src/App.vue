<template>
  <header>
    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink :to="{ name: routes.tags.name }">Tags</RouterLink>
    </nav>
    <HeaderAuth />
  </header>

  <RouterView />
</template>
<script setup>
import HeaderAuth from '@/components/header/HeaderAuth.vue';
import { routes } from '@/router/routes';
</script>

<style scoped lang="scss">
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
}
</style>
