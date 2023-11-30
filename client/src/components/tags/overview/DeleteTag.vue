<template>
  <AppAlertDialog
    :open="open"
    @close="emit('close')"
  >
    <div v-if="mutation.isPending.value === true">
      <p>Tag deletion was requested, waiting for confirmation...</p>
    </div>
    <div
      v-else-if="mutation.isSuccess.value === true"
      class="success"
    >
      <p>Tag was deleted successfully.</p>
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
        Something went wrong while trying to delete the tag, please reload the page and try again.
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
        Delete tag <em>"{{ tag.name }}"</em>?
      </DialogTitle>

      <div class="actions">
        <button
          type="button"
          @click="emit('close')"
        >
          No, keep tag
        </button>
        <button
          type="button"
          class="btn-delete"
          @click="deleteTag"
        >
          Yes, delete
        </button>
      </div>
    </template>
  </AppAlertDialog>
</template>

<script setup>
import AppAlertDialog from '@/components/common/dialogs/AppAlertDialog.vue';
import { useChoreApiDeleteTag } from '@/composables/queries/chores-api';
import { useQueryClient } from '@tanstack/vue-query';
import { DialogTitle } from '@headlessui/vue';

const props = defineProps({
  open: {
    type: Boolean,
    default: false,
  },
  tag: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(['close']);

//#region delete

const queryClient = useQueryClient();
const mutation = useChoreApiDeleteTag(queryClient);

const deleteTag = async () => {
  try {
    await mutation.mutateAsync(props.tag.id);
    // TODO toast
    emit('close');
  } catch (e) {
    console.error(e);
    // TODO toast
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
