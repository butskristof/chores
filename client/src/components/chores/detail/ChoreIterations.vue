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
    </div>
  </div>

  <EditChoreIteration
    v-if="showEditDialog"
    :open="true"
    :chore-id="chore.id"
    :iteration="iterationForEdit"
    @close="closeEditDialog"
  />

  <DeleteChoreIteration
    v-if="iterationForDelete != null"
    :open="true"
    :iteration="iterationForDelete"
    :chore-id="chore.id"
    @close="setIterationForDelete(null)"
  />

  <ul>
    <li
      v-for="iteration in chore.iterations"
      :key="iteration.id"
    >
      <div class="iteration">
        <div class="content">
          <div>{{ iteration.date }}</div>
          <div v-if="!stringIsNullOrWhitespace(iteration.notes)">{{ iteration.notes }}</div>
        </div>
        <div class="actions">
          <button
            type="button"
            @click="setIterationForDelete(iteration.id)"
          >
            delete
          </button>
        </div>
      </div>
    </li>
  </ul>
</template>

<script setup>
import { ref } from 'vue';
import EditChoreIteration from '@/components/chores/detail/EditChoreIteration.vue';
import { stringIsNullOrWhitespace } from '@/utilities/string';
import DeleteChoreIteration from '@/components/chores/detail/DeleteChoreIteration.vue';

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

const iterationForDelete = ref(null);
const setIterationForDelete = (id) =>
  (iterationForDelete.value = id == null ? null : props.chore.iterations.find((i) => i.id === id));

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

.iteration {
  display: flex;
  flex-direction: row;
  justify-content: space-between;

  .actions {
    display: flex;
    flex-direction: row;
    align-items: flex-start;
    gap: 0.5rem;
  }
}
</style>
