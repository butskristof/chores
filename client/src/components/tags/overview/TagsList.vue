<template>
  <div class="header">
    <div class="title">
      <h2>Tags</h2>
    </div>
    <div class="actions">
      <button
        type="button"
        @click="() => showEditDialog()"
      >
        +
      </button>
    </div>
  </div>

  <EditTag
    :open="editDialogOpen"
    :tag="tagForEdit"
    @close="closeEditDialog"
  />

  <DeleteTag
    :open="tagForDelete != null"
    @close="closeDeleteDialog"
  />

  <div class="tags-list">
    <QueryData
      v-slot="{ data }"
      :query="tagsQuery"
    >
      <TagsListItem
        v-for="tag in data.value"
        :key="tag.id"
        :tag="tag"
        @edit="showEditDialog(tag)"
        @delete="showDeleteDialog(tag)"
      />
    </QueryData>
  </div>
</template>

<script setup>
import { useChoresApiTags } from '@/composables/queries/chores-api';
import QueryData from '@/components/common/QueryData.vue';
import TagsListItem from '@/components/tags/overview/TagsListItem.vue';
import EditTag from '@/components/tags/overview/EditTag.vue';
import { ref } from 'vue';
import DeleteTag from '@/components/tags/overview/DeleteTag.vue';

const tagsQuery = useChoresApiTags();

//#region edit & delete

const editDialogOpen = ref(false);
const tagForDelete = ref(null);
const tagForEdit = ref(null);

const showEditDialog = (tag = null) => {
  tagForEdit.value = tag;
  editDialogOpen.value = true;
};
const closeEditDialog = () => {
  editDialogOpen.value = false;
  tagForEdit.value = null;
};

const showDeleteDialog = (tag) => (tagForDelete.value = tag);
const closeDeleteDialog = () => (tagForDelete.value = null);

//#endregion
</script>

<style scoped lang="scss">
.header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
}
</style>
