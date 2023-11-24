<template>
  <div class="auth">
    <div v-if="!authStore.isAuthenticated">
      <button
        type="button"
        @click="login"
      >
        Log in
      </button>
    </div>
  </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/auth';
import authService from '@/services/auth/auth.service';
import { useRoute } from 'vue-router';

const authStore = useAuthStore();
const route = useRoute();

const login = () => {
  authService.login(route.fullPath);
};
</script>

<style scoped></style>
