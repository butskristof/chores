<template>
  <h1>Chores</h1>
  <main>
    <div>Hello, world!</div>
    <div>isAuthenticated: {{ authStore.isAuthenticated }}</div>
    <div>
      access token:
      <pre>{{ JSON.stringify(authStore.accessToken, null, 2) }}</pre>
    </div>
  </main>
</template>

<script setup>
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();
</script>
