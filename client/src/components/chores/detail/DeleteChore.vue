<template>
  <DeleteDialog
    entity="chore"
    :is-loading="mutation.isPending.value"
    :is-error="mutation.isError.value"
    :is-success="mutation.isSuccess.value"
    :error="mutation.error.value"
    @close="$emit('close')"
    @delete="deleteChore"
  >
    <template #confirm-text>
      Delete chore
      <strong>{{ chore.name }}</strong>
      ?
    </template>
  </DeleteDialog>
</template>

<script setup>
import { useQueryClient } from '@tanstack/vue-query';
import { useChoresApiDeleteChore } from '@/composables/queries/chores-api.js';
import { useToast } from 'vue-toastification';
import DeleteDialog from '@/components/common/DeleteDialog.vue';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(['close']);

const toast = useToast();
const queryClient = useQueryClient();
const mutation = useChoresApiDeleteChore(queryClient);

const deleteChore = async () => {
  try {
    await mutation.mutateAsync(props.chore.id);
    toast.success('Chore deleted');
    emit('close', true);
  } catch (e) {
    // error should be printed to dialog as well
    console.error(e);
  }
};
</script>
