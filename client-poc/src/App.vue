<template>
  <header>header</header>

  <div class="app-body">
    <main>
      <RouterView />
    </main>

    <footer>footer</footer>
  </div>
</template>

<script setup>
import { RouterView } from 'vue-router';
</script>
