<template>
  <div
    v-if="chore"
    class="chore-detail"
  >
    <PageHeader
      :title="chore.name"
      :inline-padding="false"
      class="header"
    >
      <template #title>
        <div class="header-left">
          <h1>{{ chore.name }}</h1>
          <ChoreTags :tags="chore.tags" />
          <p>due every {{ chore.interval }} days</p>
        </div>
      </template>
      <template #actions>
        <div class="actions">
          <PrimeButton
            label="Edit tags"
            icon="pi pi-tags"
          />
          <PrimeButton
            label="Edit"
            icon="pi pi-pencil"
          />
          <PrimeButton
            label="Delete"
            icon="pi pi-trash"
            severity="danger"
          />
        </div>
      </template>
    </PageHeader>

    <h2>Notes</h2>
    <div>{{ chore.notes }}</div>

    <h2>Iterations</h2>

    <DebugValue :value="chore" />
  </div>
</template>

<script setup>
import DebugValue from '@/components/debug/DebugValue.vue';
import { useRouteParams } from '@vueuse/router';
import { useChoresApiChore } from '@/composables/queries/chores-api.js';
import { computed } from 'vue';
import PrimeButton from 'primevue/button';
import PageHeader from '@/components/common/PageHeader.vue';
import ChoreTags from '@/components/chores/common/ChoreTags.vue';

const choreId = useRouteParams('id');
const choreQuery = useChoresApiChore(choreId);
const chore = computed(() => choreQuery.data.value);
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.header {
  align-items: flex-start;
}

.header-left {
  @include flex-column;
  gap: 1rem;
}

.actions {
  @include flex-row;
  flex-wrap: wrap;
  justify-content: flex-end;
  gap: 1rem;
}
</style>
