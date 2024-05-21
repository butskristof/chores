<template>
  <DeleteDialog
    entity="iteration"
    :is-success="mutation.isSuccess.value"
    :is-error="mutation.isError.value"
    :is-loading="mutation.isPending.value"
    :error="mutation.error.value"
    @close="$emit('close')"
    @delete="deleteIteration"
  >
    <template #confirm-text>
      Delete iteration on
      <strong>{{ formatDate(iteration.date) }}</strong
      >?
    </template>
  </DeleteDialog>
</template>

<script setup>
import DeleteDialog from '@/components/common/DeleteDialog.vue';
import { useQueryClient } from '@tanstack/vue-query';
import { useToast } from 'vue-toastification';
import { useChoresApiDeleteChoreIteration } from '@/composables/queries/chores-api.js';
import { formatDate } from '@/utilities/datetime.js';

const props = defineProps({
  choreId: {
    type: String,
    required: true,
  },
  iteration: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(['close']);

const toast = useToast();
const queryClient = useQueryClient();

const mutation = useChoresApiDeleteChoreIteration(queryClient);
const deleteIteration = async () => {
  try {
    await mutation.mutateAsync({ choreId: props.choreId, iterationId: props.iteration.id });
    toast.success('Iteration deleted');
    emit('close');
  } catch (e) {
    // error should be printed to dialog as well
    console.error(e);
  }
};
</script>
