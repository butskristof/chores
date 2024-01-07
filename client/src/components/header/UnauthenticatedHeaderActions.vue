<template>
  <button
    type="button"
    @click="authService.login"
  >
    Log in
  </button>
</template>

<script setup>
import authService from '@/services/auth/auth.service.js';
</script>

<style scoped lang="scss"></style>
