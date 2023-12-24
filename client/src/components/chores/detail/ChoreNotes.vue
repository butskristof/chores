<template>
  <div class="header">
    <h2>Notes</h2>
    <div class="actions">
      <button
        v-if="!isEdit"
        type="button"
        @click="showEdit"
      >
        edit
      </button>
    </div>
  </div>
  <div
    v-if="isEdit"
    class="edit"
  >
    <textarea v-model.trim="notes" />
    <div class="actions">
      <button
        type="button"
        @click="cancelEdit"
      >
        cancel
      </button>
      <button
        type="button"
        @click="saveEdit"
      >
        save
      </button>
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
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}

.edit {
  .actions {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
    gap: 0.5rem;
  }
}
</style>
