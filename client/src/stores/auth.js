import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null);

  //#region getters

  const isAuthenticated = computed(() => user.value != null && user.value.expired !== true);
  const accessToken = computed(() =>
    isAuthenticated.value === true ? user.value.access_token : null,
  );
  const profile = computed(() => (isAuthenticated.value === true ? user.value.profile : null));

  //#endregion

  //#region actions

  const setUser = (value) => (user.value = value);

  //#endregion

  return {
    isAuthenticated,
    profile,
    accessToken,

    setUser,
  };
});
