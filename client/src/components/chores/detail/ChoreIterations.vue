<template>
  <div class="chore-iterations">
    <PageHeader :inline-padding="false">
      <template #title>
        <h2>Iterations</h2>
      </template>
      <template #actions>
        <div class="actions">
          <PrimeButton
            label="Add iteration"
            icon="pi pi-plus"
            @click="openEditDialog()"
          />
        </div>
      </template>
    </PageHeader>

    <div
      v-if="chore.iterations.length > 0"
      class="iterations"
    >
      <div
        v-for="iteration in chore.iterations"
        :key="iteration.id"
        class="iteration"
      >
        <div class="details">
          <div class="date">{{ format(new Date(iteration.date), 'dd/MM/yyyy') }}</div>
          <div
            v-if="!stringIsNullOrWhitespace(iteration.notes)"
            class="notes"
          >
            {{ iteration.notes }}
          </div>
        </div>
        <div class="actions">
          <PrimeButton
            icon="pi pi-trash"
            severity="danger"
            @click="iterationForDelete = iteration"
          />
        </div>
        <PrimeDivider v-if="false" />
      </div>
    </div>
    <div v-else>This chore doesn't have any iterations registered.</div>

    <EditIteration
      v-if="showEditDialog"
      :iteration="iterationForEdit"
      :chore-id="chore.id"
      @close="closeEditDialog"
    />
    <DeleteIteration
      v-if="iterationForDelete != null"
      :iteration="iterationForDelete"
      :chore-id="chore.id"
      @close="iterationForDelete = null"
    />
  </div>
</template>

<script setup>
import PageHeader from '@/components/common/PageHeader.vue';
import PrimeButton from 'primevue/button';
import { ref } from 'vue';
import EditIteration from '@/components/chores/detail/EditIteration.vue';
import { stringIsNullOrWhitespace } from '@/utilities/string.js';
import PrimeDivider from 'primevue/divider';
import { format } from 'date-fns';
import DeleteIteration from '@/components/chores/detail/DeleteIteration.vue';

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
//#endregion
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.iterations {
  @include flex-column;
  gap: 1rem;
  .iteration {
    @include flex-row-justify-between;
    border-bottom: 1px solid var(--border-color);
    padding-bottom: 1rem;

    .details {
      flex-grow: 1;
    }
  }
}
</style>
