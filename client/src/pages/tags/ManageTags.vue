<template>
  <div class="header">
    <div class="title">
      <h2>Tags</h2>
    </div>
    <div class="actions">
      <button
        type="button"
        @click="showEditDialog = true"
      >
        create new
      </button>
    </div>
  </div>

  <EditTag
    :open="showEditDialog"
    @close="showEditDialog = false"
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
      :tags="data.value"
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
const showEditDialog = ref(true);
//#endregion

//#region delete

const tagForDelete = ref(null);
const setTagForDelete = (id) => {
  tagForDelete.value = id == null ? null : tagsQuery.data.value?.find((t) => t.id === id);
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

  .title h2 {
    margin: auto;
  }
}
</style>
