<template>
  <PrimeDialog
    :visible="true"
    modal
    :draggable="false"
    :style="{ width: isError ? '40rem' : '25rem', 'max-width': '95vw' }"
    :show-header="!isSuccess"
    :header="isError ? '&nbsp;' : ''"
    :closable="isError"
    class="dialog"
    @update:visible="updateVisible"
  >
    <div
      v-if="mutation.isSuccess.value === true"
      class="success"
    >
      <PrimeInlineMessage severity="success">Tag deleted</PrimeInlineMessage>
    </div>
    <div
      v-else-if="mutation.isError.value === true"
      class="error"
    >
      <p>Deleting the tag failed, please refer to the error information below for more details.</p>
      <ApiError :error="mutation.error.value" />
    </div>
    <template v-else>
      <div class="confirm-text">
        Delete tag
        <strong>{{ tag.name }}</strong>
        ?
      </div>
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
import { useToast } from 'vue-toastification';
import PrimeDialog from 'primevue/dialog';
import PrimeInlineMessage from 'primevue/inlinemessage';
import PrimeButton from 'primevue/button';
import ApiError from '@/components/common/ApiError.vue';
import { computed } from 'vue';

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
const isError = computed(() => mutation.isError.value === true);
const isSuccess = computed(() => mutation.isSuccess.value === true);
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

const updateVisible = (value) => {
  if (value === false) emit('close');
};
</script>

<style scoped lang="scss">
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';

.success {
  padding-top: 1.5rem;
  text-align: center;
}

.error {
  @include flex-column;
  gap: 1rem;
}

.confirm-text {
  text-align: center;
  font-size: 110%;
  margin-bottom: 2rem;
}

.actions {
  width: 100%;
  @include flex-row;
  gap: 0.5rem;
  justify-content: space-between;
}
</style>
