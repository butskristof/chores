<template>
  <div class="chore-notes">
    <LeftRightHeader>
      <template #left>
        <h2>Notes</h2>
      </template>
      <template #right>
        <div class="actions">
          <PrimeButton
            v-if="!showEdit"
            label="Edit notes"
            icon="pi pi-pencil"
            @click="startEdit"
          />
        </div>
      </template>
    </LeftRightHeader>

    <div
      v-if="showEdit"
      class="edit-mode"
    >
      <PrimeTextarea
        v-model.trim="notes"
        :rows="5"
        auto-resize
        class="editor"
      />
      <div class="footer">
        <div class="actions">
          <PrimeButton
            severity="secondary"
            label="Cancel"
            icon="pi pi-times"
            :disabled="mutation.isPending.value"
            @click="showEdit = false"
          />
          <PrimeButton
            label="Save"
            icon="pi pi-save"
            :loading="mutation.isPending.value"
            @click="save"
          />
        </div>
      </div>
    </div>

    <div v-else>
      <div v-if="!stringIsNullOrWhitespace(chore.notes)">{{ chore.notes }}</div>
      <div
        v-else
        class="no-notes"
      >
        This chore doesn't have any notes attached.
      </div>
    </div>
  </div>
</template>

<script setup>
import { stringIsNullOrWhitespace } from '@/utilities/string.js';
import LeftRightHeader from '@/components/common/LeftRightHeader.vue';
import PrimeButton from 'primevue/button';
import { ref } from 'vue';
import { useQueryClient } from '@tanstack/vue-query';
import { useChoresApiUpdateChoreNotes } from '@/composables/queries/chores-api.js';
import PrimeTextarea from 'primevue/textarea';
import { useToast } from 'vue-toastification';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

const showEdit = ref(false);

const notes = ref('');
const startEdit = () => {
  notes.value = props.chore.notes;
  showEdit.value = true;
};

const toast = useToast();
const queryClient = useQueryClient();
const mutation = useChoresApiUpdateChoreNotes(queryClient);

const save = async () => {
  try {
    const payload = {
      choreId: props.chore.id,
      notes: notes.value,
    };
    await mutation.mutateAsync(payload);
    toast.success('Chore notes updated');
    showEdit.value = false;
  } catch (e) {
    console.error(e);
  }
};
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.actions {
  @include flex-row-actions;
}

.edit-mode {
  @include flex-column;
  gap: 1rem;
  .editor {
    width: 100%;
  }

  .footer {
    @include flex-row;
    justify-content: flex-end;
  }
}
</style>
