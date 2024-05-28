import { useQuery } from '@tanstack/vue-query';
import bffService from '@/services/bff.service.js';
import { computed } from 'vue';

export const useBffUser = () => {
  const query = useQuery({
    queryKey: ['bff', 'user'],
    queryFn: bffService.getUser,
    staleTime: Infinity,
    cacheTime: Infinity,
    retry: false,
  });
  const isLoading = computed(() => query.isPending.value);
  const isAuthenticated = computed(() => query.isSuccess.value === true);
  const claims = computed(() =>
    query.data.value?.reduce((u, c) => {
      u[c.type] = c.value;
      return u;
    }, {}),
  );
  const name = computed(() => claims.value.name);
  const sub = computed(() => claims.value.sub);
  return { query, isLoading, isAuthenticated, claims, name, sub };
};

export const useBffDiagnostics = () =>
  useQuery({
    queryKey: ['bff', 'diagnostics'],
    queryFn: bffService.getDiagnostics,
  });
