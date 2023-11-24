<template>
  <h1>Sign in</h1>
</template>

<script setup>
import { useRouter } from 'vue-router';
import authService from '@/services/auth/auth.service';

const router = useRouter();
const authenticate = async () => {
  try {
    const response = await authService.callback(false);
    const returnPath = response?.state ?? '/';
    await router.push(returnPath);
  } catch (e) {
    console.error(e);
    router.push({
      name: 'auth.unauthorized',
    });
  }
};
authenticate();
</script>
