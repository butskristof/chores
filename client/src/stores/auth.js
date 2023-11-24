import { defineStore } from 'pinia';
import { computed, ref, watch } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null);

  //#region getters

  const isAuthenticated = computed(() => user.value != null && user.value.expired !== true);
  const accessToken = computed(() => (isAuthenticated.value ? user.value.access_token : null));
  const profile = computed(() => (isAuthenticated.value ? user.value.profile : null));

  //#endregion

  //#region actions

  const setUser = (value) => (user.value = value);

  watch(user, () => {
    console.log('auth store: user updated', user.value);
  });

  //#endregion

  return {
    isAuthenticated,
    profile,
    accessToken,

    setUser,
  };
});
