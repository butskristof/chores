<template>
  <AppDialog
    :open="open"
    :alert="true"
    @close="emit('close')"
  >
    <div v-if="mutation.isPending.value === true">
      <p>Iteration deletion was requested, waiting for confirmation...</p>
    </div>
    <div
      v-else-if="mutation.isSuccess.value === true"
      class="success"
    >
      <p>Iteration was deleted successfully.</p>
      <div class="actions">
        <button
          type="button"
          @click="emit('close')"
        >
          Close
        </button>
      </div>
    </div>
    <div
      v-else-if="mutation.isError.value === true"
      class="error"
    >
      <p>
        Something went wrong while trying to delete the iteration, please reload the page and try
        again.
      </p>
      <pre>{{ mutation.error.value }}</pre>
      <div class="actions">
        <button
          type="button"
          @click="emit('close')"
        >
          Close
        </button>
      </div>
    </div>
    <template v-else>
      <DialogTitle>
        Delete iteration <em>"{{ iteration.date }}"</em>?
      </DialogTitle>

      <div class="actions">
        <button
          type="button"
          @click="emit('close')"
        >
          No, keep iteration
        </button>
        <button
          type="button"
          class="btn-delete"
          @click="deleteIteration"
        >
          Yes, delete
        </button>
      </div>
    </template>
  </AppDialog>
</template>

<script setup>
import { useQueryClient } from '@tanstack/vue-query';
import { useToast } from 'vue-toastification';
import { useChoresApiDeleteChoreIteration } from '@/composables/queries/chores-api';
import AppDialog from '@/components/common/dialogs/AppDialog.vue';
import { DialogTitle } from '@headlessui/vue';

const props = defineProps({
  open: {
    type: Boolean,
    default: false,
  },
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

const queryClient = useQueryClient();
const toast = useToast();

//#region delete

const mutation = useChoresApiDeleteChoreIteration(queryClient);
const deleteIteration = async () => {
  try {
    await mutation.mutateAsync({ choreId: props.choreId, iterationId: props.iteration.id });
    toast.success('Iteration deleted');
    emit('close', true);
  } catch (e) {
    console.error(e);
  }
};

//#endregion
</script>

<style scoped lang="scss">
.error {
  pre {
    display: block;
    overflow-x: auto;
    width: 100%;
  }
}
.actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  gap: 0.5rem;

  .btn-delete {
    background-color: red;
  }
}
</style>
