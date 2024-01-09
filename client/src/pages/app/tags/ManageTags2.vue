<template>
  <div class="card manage-tags">
    <Toolbar class="toolbar">
      <template #start>
        <h3>Manage tags</h3>
      </template>
      <template #end>
        <Button
          label="New"
          icon="pi pi-plus"
          class="p-button-success"
        />
      </template>
    </Toolbar>

    <QueryData
      v-slot="{ data }"
      :query="tagsQuery"
    >
      <DataTable
        striped-rows
        removable-sort
        :value="data.value.tags"
      >
        <Column
          field="name"
          header="Name"
          :sortable="true"
        />
        <Column
          field="choresCount"
          header="Used by # chores"
          :sortable="true"
        />
        <Column header-style="min-width: 10rem;">
          <template #body>
            <div class="row-actions">
              <Button
                icon="pi pi-pencil"
                class="p-button-rounded p-button-success"
              />
              <Button
                icon="pi pi-trash"
                class="p-button-rounded p-button-danger"
              />
            </div>
          </template>
        </Column>
      </DataTable>
    </QueryData>
  </div>
</template>

<script setup>
import Toolbar from 'primevue/toolbar';
import Button from 'primevue/button';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import QueryData from '@/components/common/QueryData.vue';
import { useChoresApiTags } from '@/composables/queries/chores-api.js';

const tagsQuery = useChoresApiTags();
</script>

<style scoped lang="scss">
.toolbar {
  margin-bottom: 2rem;

  h3 {
    margin-block: initial;
  }
}

.row-actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  gap: 0.5rem;
}
</style>
