import { useAuthStore } from '@/stores/auth';

export const accessTokenGetter = () => {
  const authStore = useAuthStore();
  return authStore.accessToken;
};
