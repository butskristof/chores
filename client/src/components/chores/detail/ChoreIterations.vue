<template>
  <div class="header">
    <h2>Iterations</h2>
    <div class="actions">
      <button
        type="button"
        @click="openEditDialog"
      >
        add
      </button>
      <EditChoreIteration
        v-if="showEditDialog"
        :open="true"
        :chore-id="chore.id"
        :iteration="iterationForEdit"
        @close="closeEditDialog"
      />
    </div>
  </div>
  <ul>
    <li
      v-for="iteration in chore.iterations"
      :key="iteration.id"
    >
      <div>{{ iteration.date }}</div>
      <div v-if="!stringIsNullOrWhitespace(iteration.notes)">{{ iteration.notes }}</div>
    </li>
  </ul>
</template>

<script setup>
import { ref } from 'vue';
import EditChoreIteration from '@/components/chores/detail/EditChoreIteration.vue';
import { stringIsNullOrWhitespace } from '@/utilities/string';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

//#region edit
const showEditDialog = ref(false);
const iterationForEdit = ref(null);
const openEditDialog = (id = null) => {
  iterationForEdit.value = id == null ? null : props.chore.iterations.find((i) => i.id === id);
  showEditDialog.value = true;
};
const closeEditDialog = () => {
  showEditDialog.value = false;
  iterationForEdit.value = null;
};
//#endregion

//#region delete
//#endregion
</script>

<style scoped lang="scss">
.header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;

  .actions {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
    align-items: flex-start;
  }
}
</style>
