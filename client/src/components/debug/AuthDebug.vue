<template>
  <h2>Auth</h2>
  <h3>Profile</h3>
  <div>
    <pre>{{ JSON.stringify(authStore.profile, null, 2) }}</pre>
    <p>Profile expires {{ formattedProfileExpiration }}</p>
  </div>
  <h3>Access token</h3>
  <div>
    <pre class="raw-token">{{ authStore.accessToken }}</pre>
    <pre>{{ parsedToken }}</pre>
    <p>Access token expires {{ formattedAccessTokenExpiration }}</p>
  </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/auth';
import { computed, ref } from 'vue';
import { formatDistance, formatDistanceStrict } from 'date-fns';
const authStore = useAuthStore();

const now = ref(new Date());
setInterval(() => (now.value = new Date()), 1000);

//#region profile

const profileExpiration = computed(() => new Date(authStore.profile.exp * 1000));
const formattedProfileExpiration = computed(() =>
  formatDistance(profileExpiration.value, now.value, { addSuffix: true }),
);

//#endregion

//#region access token

const parseJwt = (token) => {
  // eslint-disable-next-line prefer-destructuring
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/gu, '+').replace(/_/gu, '/');
  const jsonPayload = decodeURIComponent(
    window
      .atob(base64)
      .split('')
      .map((c) => `%${`00${c.charCodeAt(0).toString(16)}`.slice(-2)}`)
      .join(''),
  );

  return JSON.parse(jsonPayload);
};

const parsedToken = computed(() => parseJwt(authStore.accessToken));
const accessTokenExp = computed(() => new Date(parsedToken.value.exp * 1000));
const formattedAccessTokenExpiration = computed(() =>
  formatDistanceStrict(accessTokenExp.value, now.value, { addSuffix: true, unit: 'second' }),
);
//#endregion
</script>

<style scoped lang="scss">
.raw-token {
  padding: 0.5rem;
  overflow-x: auto;
}
</style>
