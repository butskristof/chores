<template>
  <div class="chores-overview">
    <LeftRightHeader class="page-header">
      <template #left>
        <h1>Chores</h1>
      </template>
      <template #right>
        <div class="actions">
          <PrimeButton
            label="Add new chore"
            icon="pi pi-plus"
            @click="showCreate = true"
          />
        </div>
      </template>
    </LeftRightHeader>

    <ChoresList
      :chores="chores"
      :loading="queryPending"
    />

    <EditChore
      v-if="showCreate"
      @close="showCreate = false"
    />
  </div>
</template>

<script setup>
import LeftRightHeader from '@/components/common/LeftRightHeader.vue';
import PrimeButton from 'primevue/button';
import { ref } from 'vue';
import EditChore from '@/components/chores/common/EditChore.vue';
import ChoresList from '@/components/chores/overview/ChoresList.vue';
import { useChoresApiChores } from '@/composables/queries/chores-api.js';

const { chores, isPending: queryPending } = useChoresApiChores();
const showCreate = ref(false);
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.page-header {
  // .header is also used inside the component, this must be another name
  // to make sure it gets a higher importance
  padding-inline: var(--default-padding);

  .actions {
    @include flex-row-actions;
  }
}
</style>
