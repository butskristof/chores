<template>
  <h1>Sign in</h1>
</template>

<script setup>
import { useRouter } from 'vue-router';
import authService from '@/services/auth/auth.service';
import { routes } from '@/router/routes';

// this page is the 'return uri' for the OIDC flow, we expect to only get here when
// redirected by the STS. If that's the case, the URL will contain the necessary parameters
// to actually sign in the user into the application

const router = useRouter();
const authenticate = async () => {
  try {
    const response = await authService.callback(false);
    // try to retrieve a return path set by the login flow, or fall back to the application root
    const returnPath = response?.state ?? { name: routes.home.name };
    await router.push(returnPath);
  } catch (e) {
    console.error(e);
    router.push({
      name: routes.auth.children.unauthorized.name,
    });
  }
};
authenticate();
</script>
