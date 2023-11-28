import { useQuery } from '@tanstack/vue-query';
import choresApiService from '@/services/chores-api.service';

export const CHORES_API_QUERY_KEYS = {
  TAGS: {
    GET: ['tags'],
  },
};

export const useChoresApiTags = () =>
  useQuery({
    queryKey: CHORES_API_QUERY_KEYS.TAGS.GET,
    queryFn: choresApiService.getTags,
  });
