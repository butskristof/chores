<template>
  <Toolbar class="header">
    <template #start>
      <h2>Iterations</h2>
    </template>

    <template #end>
      <div class="actions">
        <Button
          type="button"
          label="Add"
          icon="pi pi-plus"
          @click="openEditDialog"
        />
      </div>
    </template>
  </Toolbar>

  <EditChoreIteration
    v-if="showEditDialog"
    :chore-id="chore.id"
    :iteration="iterationForEdit"
    @close="closeEditDialog"
  />

  <DeleteChoreIteration
    v-if="iterationForDelete != null"
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
          <Button
            type="button"
            label="Delete"
            icon="pi pi-trash"
            severity="danger"
            @click="setIterationForDelete(iteration.id)"
          />
        </div>
      </div>
    </li>
  </ul>
</template>

<script setup>
import { computed, ref } from 'vue';
import EditChoreIteration from '@/components/chores/detail/EditChoreIteration.vue';
import { stringIsNullOrWhitespace } from '@/utilities/string';
import DeleteChoreIteration from '@/components/chores/detail/DeleteChoreIteration.vue';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

const iterations = computed(() => props.chore.iterations ?? []);

//#region edit
const showEditDialog = ref(false);
const iterationForEdit = ref(null);
const openEditDialog = (id = null) => {
  iterationForEdit.value = id == null ? null : iterations.value.find((i) => i.id === id);
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
  (iterationForDelete.value = id == null ? null : iterations.value.find((i) => i.id === id));

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
.iteration {
  display: flex;
  flex-direction: row;
  justify-content: space-between;

  margin-bottom: 1rem;

  .actions {
    display: flex;
    flex-direction: row;
    align-items: flex-start;
    gap: 0.5rem;
  }
}
</style>
