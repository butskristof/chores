<template>
  <PrimeDialog
    :visible="true"
    modal
    :draggable="false"
    :style="{ width: '25rem' }"
    :breakpoints="{ '575px': '95vw' }"
    :closable="false"
    @update:visible="updateVisible"
  >
    <div
      v-if="mutation.isSuccess.value === true"
      class="success"
    >
      <PrimeInlineMessage severity="success">Tag deleted</PrimeInlineMessage>
    </div>
    <div
      v-else
      class="confirm-text"
    >
      Delete tag
      <strong>{{ tag.name }}</strong>
      ?
    </div>
    <template
      v-if="mutation.isSuccess.value !== true"
      #footer
    >
      <div class="actions">
        <PrimeButton
          label="No, keep tag"
          icon="pi pi-times"
          :disabled="mutation.isPending.value === true"
          @click="emit('close')"
        />
        <PrimeButton
          :label="mutation.isPending.value === true ? 'Deleting...' : 'Yes, delete'"
          icon="pi pi-trash"
          severity="danger"
          :loading="mutation.isPending.value === true"
          :disabled="mutation.isPending.value === true"
          @click="deleteTag"
        />
      </div>
    </template>
  </PrimeDialog>
</template>

<script setup>
import { useChoreApiDeleteTag } from '@/composables/queries/chores-api.js';
import { useQueryClient } from '@tanstack/vue-query';
import { sleep } from '@/utilities/sleep.js';
import { useToast } from 'vue-toastification';
import PrimeDialog from 'primevue/dialog';
import PrimeInlineMessage from 'primevue/inlinemessage';
import PrimeButton from 'primevue/button';

const props = defineProps({
  tag: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(['close']);

const queryClient = useQueryClient();
const toast = useToast();

const mutation = useChoreApiDeleteTag(queryClient);
const deleteTag = async () => {
  try {
    await sleep(1);
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
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';

.confirm-text {
  text-align: center;
  font-size: 110%;
  margin-bottom: var(--default-padding);
}

.actions {
  width: 100%;
  @include flex-row;
  gap: 0.5rem;
  justify-content: space-between;
}
</style>
