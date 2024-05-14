<template>
  <PrimeDialog
    :visible="true"
    modal
    :draggable="false"
    :style="{ width: isError ? '40rem' : '25rem' }"
    :show-header="isError"
    header=" "
    :closable="true"
    @update:visible="updateVisible"
  >
    <div
      v-if="isSuccess"
      class="success"
    >
      <PrimeInlineMessage severity="success">{{ capitalize(entity) }} deleted</PrimeInlineMessage>
    </div>
    <div
      v-else-if="isError"
      class="error"
    >
      <p>
        Deleting the {{ entity.toLowerCase() }} failed, please refer to the error information below
        for more details.
      </p>
      <ApiError :error="error" />
    </div>
    <template v-else>
      <div class="confirm-text">
        <slot name="confirm-text"></slot>
      </div>
      <div class="actions">
        <PrimeButton
          :label="'No, keep ' + entity.toLowerCase()"
          icon="pi pi-times"
          :disabled="isLoading"
          @click="$emit('close')"
        />
        <PrimeButton
          :label="isLoading ? 'Deleting...' : 'Yes, delete'"
          icon="pi pi-trash"
          severity="danger"
          :loading="isLoading"
          :disabled="isLoading"
          @click="$emit('delete')"
        />
      </div>
    </template>
  </PrimeDialog>
</template>

<!-- this component is set up to be a generic dialog for deleting entities -->
<!-- it offers more flexibility over the built-in confirmation service -->
<!-- and allows to keep handling the delete request in the component itself -->
<!-- this generic dialog should be wrapped in an entity-specific component -->

<!-- the header is only shown in case of an error, to show the close button -->
<!-- in the upper right corner. In success or undetermined state, the header -->
<!-- is hidden and replaced with padding-top -->
<!-- to avoid collapsing, the header has a constant value of a whitespace string -->

<script setup>
import PrimeDialog from 'primevue/dialog';
import PrimeInlineMessage from 'primevue/inlinemessage';
import { capitalize } from 'vue';
import ApiError from '@/components/common/ApiError.vue';
import PrimeButton from 'primevue/button';

defineProps({
  // if the name of the entity is passed it, it'll be used in communication (e.g. "Tag deleted")
  entity: {
    type: String,
    required: true,
  },
  isSuccess: {
    type: Boolean,
    required: true,
  },
  isError: {
    type: Boolean,
    required: true,
  },
  isLoading: {
    type: Boolean,
    required: true,
  },
  error: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close', 'delete']);

const updateVisible = (value) => {
  if (value === false) emit('close');
};
</script>

<style scoped lang="scss">
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';

.success,
.confirm-text {
  padding-top: 1.5rem;
}

.success {
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
