<template>
  <div class="manage-tags">
    <LeftRightHeader class="page-header">
      <template #left>
        <h1>Tags</h1>
      </template>
      <template #right>
        <div class="actions">
          <PrimeButton
            label="Add new tag"
            icon="pi pi-plus"
            @click="openEditDialog()"
          />
        </div>
      </template>
    </LeftRightHeader>

    <TagsList
      :tags="tags"
      :loading="queryPending"
      @edit="openEditDialog"
      @delete="setTagForDelete"
    />

    <EditTag
      v-if="showEditDialog"
      :tag="tagForEdit"
      @close="closeEditDialog()"
    />
    <DeleteTag
      v-if="tagForDelete != null"
      :tag="tagForDelete"
      @close="closeDeleteDialog()"
    />
  </div>
</template>

<script setup>
import TagsList from '@/components/tags/manage/TagsList.vue';
import { useChoresApiTags } from '@/composables/queries/chores-api.js';
import { ref } from 'vue';
import EditTag from '@/components/tags/manage/EditTag.vue';
import DeleteTag from '@/components/tags/manage/DeleteTag.vue';
import PrimeButton from 'primevue/button';
import LeftRightHeader from '@/components/common/LeftRightHeader.vue';

const { tags, isPending: queryPending } = useChoresApiTags();

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
const closeDeleteDialog = () => (tagForDelete.value = null);

//#endregion
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.page-header {
  padding-inline: var(--default-padding);

  .actions {
    @include flex-row-actions;
  }
}
</style>
