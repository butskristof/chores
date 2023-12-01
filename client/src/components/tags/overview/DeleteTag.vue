<template>
  <AppDialog
    :open="open"
    :alert="true"
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
  </AppDialog>
</template>

<script setup>
import { useChoreApiDeleteTag } from '@/composables/queries/chores-api';
import { useQueryClient } from '@tanstack/vue-query';
import { DialogTitle } from '@headlessui/vue';
import { useToast } from 'vue-toastification';
import AppDialog from '@/components/common/dialogs/AppDialog.vue';

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

const queryClient = useQueryClient();
const toast = useToast();

//#region delete

const mutation = useChoreApiDeleteTag(queryClient);

const deleteTag = async () => {
  try {
    await mutation.mutateAsync(props.tag.id);
    // TODO toast
    toast.success('Tag deleted');
    emit('close');
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
