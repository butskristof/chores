<template>
  <Dialog
    :visible="true"
    modal
    :draggable="false"
    :style="{ width: '50rem' }"
    :breakpoints="{ '1199px': '75vw', '575px': '90vw' }"
    :closable="false"
    @update:visible="updateVisible"
  >
    <div
      v-if="mutation.isSuccess.value === true"
      class="success"
    >
      <InlineMessage severity="success">Tag deleted</InlineMessage>
    </div>
    <p v-else>
      Delete tag
      <strong>{{ tag.name }}</strong>
      ?
    </p>
    <template
      v-if="mutation.isSuccess.value !== true"
      #footer
    >
      <div class="actions">
        <Button
          type="button"
          label="No, keep tag"
          icon="pi pi-times"
          class="p-button-text"
          :disabled="mutation.isPending.value === true"
          @click="emit('close')"
        />
        <Button
          type="button"
          :label="mutation.isPending.value === true ? 'Deleting...' : 'Yes, delete'"
          icon="pi pi-trash"
          severity="danger"
          :loading="mutation.isPending.value === true"
          :disabled="mutation.isPending.value === true"
          @click="deleteTag"
        />
      </div>
    </template>
  </Dialog>
</template>

<script setup>
import { useQueryClient } from '@tanstack/vue-query';
import { useChoreApiDeleteTag } from '@/composables/queries/chores-api.js';
import { useToast } from 'vue-toastification';

const props = defineProps({
  tag: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);

const queryClient = useQueryClient();
const toast = useToast();

const mutation = useChoreApiDeleteTag(queryClient);

const deleteTag = async () => {
  try {
    await mutation.mutateAsync(props.tag.id);
    toast.success('Tag deleted');
    emit('close');
  } catch (e) {
    console.error(e);
  }
};

const updateVisible = (value) => {
  if (value === false) emit('close');
};
</script>

<style scoped lang="scss">
p {
  text-align: center;
  font-size: 1.25rem;
}

.actions {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}

.success {
  display: flex;
  justify-content: center;
}
</style>
