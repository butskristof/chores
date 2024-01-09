<template>
  <div class="card">
    <Toolbar class="header">
      <template #start>
        <h3>Manage tags</h3>
      </template>
      <template #end>
        <Button
          label="New"
          icon="pi pi-plus"
          class="p-button-success"
          @click="openEditDialog"
        />
      </template>
    </Toolbar>

    <DataTable
      striped-rows
      removable-sort
      :value="tags"
      :loading="tagsQuery.isLoading.value"
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

    <EditTag v-model:visible="showEditDialog" />
  </div>
</template>

<script setup>
import Toolbar from 'primevue/toolbar';
import Button from 'primevue/button';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import { useChoresApiTags } from '@/composables/queries/chores-api.js';
import { computed, ref } from 'vue';
import EditTag from '@/components/tags/manage/EditTag.vue';

const tagsQuery = useChoresApiTags();
const tags = computed(() => tagsQuery.data.value?.tags ?? []);

//#region edit

const showEditDialog = ref(true);
const tagForEdit = ref(null);
const openEditDialog = (id = null) => {
  tagForEdit.value = id == null ? null : tags.value.find((t) => t.id === id);
  showEditDialog.value = true;
};
// const closeEditDialog = () => {
//   showEditDialog.value = false;
//   tagForEdit.value = null;
// };

//#endregion
</script>

<style scoped lang="scss">
.header {
  margin-bottom: 2rem;

  h3 {
    margin-block: 0;
  }
}

.row-actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  gap: 0.5rem;
}
</style>
