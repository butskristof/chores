<template>
  <div class="card">
    <Toolbar class="header">
      <template #start>
        <h1>Manage tags</h1>
      </template>
      <template #end>
        <Button
          label="New"
          icon="pi pi-plus"
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
      <Column header-style="min-width: 2rem;">
        <template #body="slotProps">
          <div class="row-actions">
            <Button
              icon="pi pi-pencil"
              class="p-button-rounded"
              @click="openEditDialog(slotProps.data.id)"
            />
            <Tippy
              :content="
                slotProps.data.choresCount > 0
                  ? 'Tags which are referenced by chores cannot be deleted'
                  : null
              "
              placement="auto-end"
            >
              <Button
                icon="pi pi-trash"
                class="p-button-rounded p-button-danger"
                :disabled="slotProps.data.choresCount > 0"
                @click="setTagForDelete(slotProps.data.id)"
              />
            </Tippy>
          </div>
        </template>
      </Column>
    </DataTable>

    <EditTag
      v-if="showEditDialog"
      :tag="tagForEdit"
      @close="closeEditDialog"
    />

    <DeleteTag
      v-if="tagForDelete != null"
      :tag="tagForDelete"
      @close="setTagForDelete(null)"
    />
  </div>
</template>

<script setup>
import { useChoresApiTags } from '@/composables/queries/chores-api.js';
import { computed, ref } from 'vue';
import EditTag from '@/components/tags/manage/EditTag.vue';
import DeleteTag from '@/components/tags/manage/DeleteTag.vue';
import { Tippy } from 'vue-tippy';

const tagsQuery = useChoresApiTags();
const tags = computed(() => tagsQuery.data.value?.tags ?? []);

//#region edit

const showEditDialog = ref(false);
const tagForEdit = ref(null);
const openEditDialog = (id = null) => {
  tagForEdit.value = id == null ? null : tags.value.find((t) => t.id === id);
  showEditDialog.value = true;
};
const closeEditDialog = () => {
  showEditDialog.value = false;
  tagForEdit.value = null;
};

//#endregion

//#region delete

const tagForDelete = ref(null);
const setTagForDelete = (id) =>
  (tagForDelete.value = id == null ? null : tags.value.find((t) => t.id === id));

//#endregion
</script>

<style scoped lang="scss">
.header {
  margin-bottom: 2rem;

  h1 {
    font-size: 1.75rem;
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
