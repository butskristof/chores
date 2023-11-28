<template>
  <h1>Sign in</h1>
</template>

<script setup>
import authService from '@/services/auth/auth.service';

authService.callback(true).catch((e) => console.error(e));
</script>

<style scoped lang="scss"></style>
