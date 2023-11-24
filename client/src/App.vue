<template>
  <header>
    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/about">About</RouterLink>
    </nav>
    <HeaderAuth />
  </header>

  <RouterView />
</template>
<script setup>
import HeaderAuth from '@/components/header/HeaderAuth.vue';
</script>

<style scoped lang="scss">
header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}
</style>
