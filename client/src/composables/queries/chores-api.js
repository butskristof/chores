import { useMutation, useQuery } from '@tanstack/vue-query';
import choresApiService from '@/services/chores-api.service';
import { computed } from 'vue';
import { addPropsToChore } from '@/utilities/chores.js';

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

export const useChoresApiChores = () => {
  const query = useQuery({
    queryKey: CHORES_API_QUERY_KEYS.CHORES.GET,
    queryFn: choresApiService.getChores,
  });
  query.chores = computed(() => query.data.value?.chores.map(addPropsToChore) ?? []);
  return query;
};

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

export const useChoresApiChore = (id) => {
  const query = useQuery({
    queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(id),
    queryFn: () => choresApiService.getChore(id.value),
  });
  query.chore = computed(() =>
    query.data.value == null ? null : addPropsToChore(query.data.value),
  );
  return query;
};

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
    mutationFn: choresApiService.updateChoreNotes,
    onSuccess: (response, request) => {
      queryClient.invalidateQueries({
        queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(request.choreId),
      });
    },
  });

export const useChoresApiUpdateChoreTags = (queryClient) =>
  useMutation({
    mutationFn: (payload) => choresApiService.updateChoreTags(payload),
    onSuccess: (response, request) => {
      queryClient.invalidateQueries({
        queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(request.choreId),
      });
      queryClient.invalidateQueries({
        queryKey: CHORES_API_QUERY_KEYS.CHORES.GET,
      });
      queryClient.invalidateQueries({
        queryKey: CHORES_API_QUERY_KEYS.TAGS.GET,
      });
    },
  });

export const useChoresApiUpsertChoreIteration = (queryClient) =>
  useMutation({
    mutationFn: (payload) =>
      payload.iterationId
        ? choresApiService.updateChoreIteration(payload)
        : choresApiService.createChoreIteration(payload),
    onSuccess: (response, request) => {
      queryClient.invalidateQueries({ queryKey: CHORES_API_QUERY_KEYS.CHORES.GET });
      queryClient.invalidateQueries({
        queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(request.choreId),
      });
    },
  });

export const useChoresApiDeleteChoreIteration = (queryClient) =>
  useMutation({
    mutationFn: choresApiService.deleteChoreIteration,
    onSuccess: (response, request) => {
      queryClient.invalidateQueries({ queryKey: CHORES_API_QUERY_KEYS.CHORES.GET });
      queryClient.invalidateQueries({
        queryKey: CHORES_API_QUERY_KEYS.CHORES.GET_BY_ID(request.choreId),
      });
    },
  });
//#endregion

//#region tags

export const useChoresApiTags = () => {
  const query = useQuery({
    queryKey: CHORES_API_QUERY_KEYS.TAGS.GET,
    queryFn: choresApiService.getTags,
  });
  query.tags = computed(() => query.data.value?.tags ?? []);
  return query;
};

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
