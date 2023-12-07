<template>
  <h1>Unauthorized</h1>
</template>

<script setup>
import authService from '@/services/auth/auth.service';
// the user gets redirected here as a result of a failed login attempt
// as such, we'll direct them into the login flow again

// TODO maybe force a login prompt?
authService.login();
</script>
