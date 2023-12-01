import { useMutation, useQuery } from '@tanstack/vue-query';
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

export const useChoresApiUpsertTag = (queryClient) =>
  useMutation({
    mutationFn: (payload) =>
      payload.id ? choresApiService.updateTag(payload) : choresApiService.createTag(payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: CHORES_API_QUERY_KEYS.TAGS.GET });
    },
  });

export const useChoreApiDeleteTag = (queryClient) =>
  useMutation({
    mutationFn: (id) => choresApiService.deleteTag(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: CHORES_API_QUERY_KEYS.TAGS.GET });
    },
  });
