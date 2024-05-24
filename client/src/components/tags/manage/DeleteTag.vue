<template>
  <DeleteDialog
    entity="tag"
    :is-success="mutation.isSuccess.value"
    :is-error="mutation.isError.value"
    :is-loading="mutation.isPending.value"
    :error="mutation.error.value"
    @close="$emit('close')"
    @delete="deleteTag()"
  >
    <template #confirm-text>
      Delete tag
      <strong>{{ tag.name }}</strong>
      ?
    </template>
  </DeleteDialog>
</template>

<script setup>
import { useChoreApiDeleteTag } from '@/composables/queries/chores-api.js';
import { useQueryClient } from '@tanstack/vue-query';
import { useToast } from 'vue-toastification';
import DeleteDialog from '@/components/common/DeleteDialog.vue';

const props = defineProps({
  tag: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(['close']);

const toast = useToast();
const queryClient = useQueryClient();
const mutation = useChoreApiDeleteTag(queryClient);
const deleteTag = async () => {
  try {
    await mutation.mutateAsync(props.tag.id);
    toast.success('Tag deleted');
    emit('close');
  } catch (e) {
    // error should be printed to dialog as well
    console.error(e);
  }
};
</script>
