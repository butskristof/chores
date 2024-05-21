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

    <ChoreNextDue
      :chore="chore"
      class="due-next"
    />

    <PrimeTimeline
      v-if="chore.iterations.length > 0"
      :value="chore.iterations"
      class="timeline"
    >
      <template #content="{ item: iteration }">
        <div class="iteration">
          <div class="details">
            <div class="date">{{ formatDate(iteration.date) }}</div>
            <div
              v-if="!stringIsNullOrWhitespace(iteration.notes)"
              class="notes"
            >
              {{ iteration.notes }}
            </div>
          </div>
          <div class="actions">
            <Tippy
              content="Not implemented yet"
              placement="bottom-end"
            >
              <PrimeButton
                icon="pi pi-pencil"
                disabled
              />
            </Tippy>
            <PrimeButton
              icon="pi pi-trash"
              severity="danger"
              @click="iterationForDelete = iteration"
            />
          </div>
        </div>
      </template>
    </PrimeTimeline>
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
import DeleteIteration from '@/components/chores/detail/DeleteIteration.vue';
import { Tippy } from 'vue-tippy';
import ChoreNextDue from '@/components/chores/detail/ChoreNextDue.vue';
import { formatDate } from '@/utilities/datetime.js';
import PrimeTimeline from 'primevue/timeline';

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

.due-next {
  margin-bottom: 1.25rem;
}

.timeline {
  --p-timeline-vertical-event-content-padding: 0;
  :deep(.p-timeline-event-separator) {
    margin-right: 1rem;
  }

  :deep(.p-timeline-event-opposite) {
    flex: initial;
  }
}

.iteration {
  @include flex-row-justify-between;
  padding-bottom: 1rem;

  &:not(:last-of-type) {
    border-bottom: 1px solid var(--border-color);
  }

  .details {
    flex-grow: 1;
  }

  .actions {
    @include flex-row;
    gap: 0.5rem;
    align-items: flex-start;
  }
}
</style>
