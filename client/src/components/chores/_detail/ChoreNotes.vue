<!-- eslint-disable vue/no-multiple-template-root -->
<template>
  <Toolbar class="header">
    <template #start>
      <h2>Notes</h2>
    </template>
    <template #end>
      <div class="actions">
        <Button
          v-if="!isEdit"
          type="button"
          label="Edit"
          icon="pi pi-pencil"
          @click="showEdit"
        />
      </div>
    </template>
  </Toolbar>

  <div
    v-if="isEdit"
    class="edit"
  >
    <Textarea
      v-model.trim="notes"
      auto-resize
      class="input"
    />

    <div class="actions">
      <Button
        type="button"
        label="Cancel"
        icon="pi pi-times"
        class="p-button-text"
        @click="cancelEdit"
      />
      <Button
        type="button"
        label="Save"
        icon="pi pi-save"
        @click="saveEdit"
      />
    </div>
  </div>

  <div
    v-else
    class="view"
  >
    <div v-if="stringIsNullOrWhitespace(chore.notes)">
      <em>No notes have been added</em>
    </div>

    <div v-else>{{ chore.notes }}</div>
  </div>
</template>

<script setup>
import { stringIsNullOrWhitespace } from '@/utilities/string';
import { ref } from 'vue';
import { useChoresApiUpdateChoreNotes } from '@/composables/queries/chores-api';
import { useQueryClient } from '@tanstack/vue-query';
import { useToast } from 'vue-toastification';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

const queryClient = useQueryClient();
const toast = useToast();

//#region edit
const mutation = useChoresApiUpdateChoreNotes(queryClient);

const isEdit = ref(false);
const notes = ref(null);

const showEdit = () => {
  notes.value = props.chore.notes;
  isEdit.value = true;
};
const saveEdit = async () => {
  try {
    const payload = {
      choreId: props.chore.id,
      notes: notes.value,
    };
    await mutation.mutateAsync(payload);
    toast.success('Chore notes updated');
    isEdit.value = false;
    // notes.value = null;
  } catch (e) {
    console.error(e);
  }
};
const cancelEdit = () => {
  if (notes.value !== props.chore.notes) {
    // TODO alert
  }
  isEdit.value = false;
  // notes.value = null;
};
//#endregion
</script>

<style scoped lang="scss">
.header {
  margin-bottom: 1rem;

  h2 {
    //font-size: 1.75rem;
    margin-block: 0;
  }
}

.edit {
  .input {
    width: 100%;
    margin-bottom: 1rem;
    min-height: 10rem;
  }

  .actions {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
    gap: 0.5rem;
  }
}
</style>
