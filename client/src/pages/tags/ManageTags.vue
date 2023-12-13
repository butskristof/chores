<template>
  <div class="header">
    <div class="title">
      <h2>Tags</h2>
    </div>
    <div class="actions">
      <button
        type="button"
        @click="openEditDialog"
      >
        create new
      </button>
    </div>
  </div>

  <EditTag
    v-if="showEditDialog"
    :open="true"
    :tag="tagForEdit"
    @close="closeEditDialog"
  />

  <DeleteTag
    v-if="tagForDelete != null"
    :open="true"
    :tag="tagForDelete"
    @close="setTagForDelete(null)"
  />

  <QueryData
    v-slot="{ data }"
    :query="tagsQuery"
  >
    <TagsList
      :tags="data.value.tags"
      @edit="openEditDialog"
      @delete="setTagForDelete"
    />
  </QueryData>
</template>

<script setup>
import TagsList from '@/components/tags/overview/TagsList.vue';
import { useChoresApiTags } from '@/composables/queries/chores-api';
import QueryData from '@/components/common/QueryData.vue';
import { ref } from 'vue';
import DeleteTag from '@/components/tags/overview/DeleteTag.vue';
import EditTag from '@/components/tags/overview/EditTag.vue';

const tagsQuery = useChoresApiTags();

//#region edit
const showEditDialog = ref(false);
const tagForEdit = ref(null);
const openEditDialog = (id = null) => {
  tagForEdit.value = id == null ? null : tagsQuery.data.value.tags?.find((t) => t.id === id);
  showEditDialog.value = true;
};
const closeEditDialog = () => {
  showEditDialog.value = false;
  tagForEdit.value = null;
};
//#endregion

//#region delete

const tagForDelete = ref(null);
const setTagForDelete = (id) => {
  tagForDelete.value = id == null ? null : tagsQuery.data.value.tags?.find((t) => t.id === id);
};

//#endregion
</script>

<style scoped lang="scss">
.header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;

  margin-bottom: 1rem;
}
</style>
