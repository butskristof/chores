import { useMutation, useQuery } from '@tanstack/vue-query';
import choresApiService from '@/services/chores-api.service';

export const CHORES_API_QUERY_KEYS = {
  CHORES: {
    GET: ['chores', 'overview'],
    GET_BY_ID: (id) => ['chores', 'detail', id],
  },
  TAGS: {
    GET: ['tags', 'overview'],
  },
};

//#region chores

export const useChoresApiChores = () =>
  useQuery({
    queryKey: CHORES_API_QUERY_KEYS.CHORES.GET,
    queryFn: choresApiService.getChores,
  });

export const useChoresApiUpsertChore = (queryClient) =>
  useMutation({
    mutationFn: (payload) =>
      payload.id ? choresApiService.updateChore(payload) : choresApiService.createChore(payload),
    onSuccess: (response, request) => {
      queryClient.invalidateQueries({ queryKey: CHORES_API_QUERY_KEYS.CHORES.GET });
      if (request.id != null) {
        // was edit
        queryClient.invalidateQueries({
          queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(request.id),
        });
      }
    },
  });

export const useChoresApiChore = (id) =>
  useQuery({
    queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(id),
    queryFn: () => choresApiService.getChore(id.value),
  });

export const useChoresApiDeleteChore = (queryClient) =>
  useMutation({
    mutationFn: (id) => choresApiService.deleteChore(id),
    onSuccess: (response, request) => {
      queryClient.invalidateQueries({ queryKey: CHORES_API_QUERY_KEYS.CHORES.GET });
      queryClient.invalidateQueries({
        queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(request.id),
      });
    },
  });

export const useChoresApiUpdateChoreNotes = (queryClient) =>
  useMutation({
    mutationFn: (payload) => choresApiService.updateChoreNotes(payload),
    onSuccess: (response, request) => {
      queryClient.invalidateQueries({
        queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(request.choreId),
      });
    },
  });

//#endregion

//#region tags

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

//#endregion
