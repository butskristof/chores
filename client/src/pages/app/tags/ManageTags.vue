<template>
  <div class="manage-tags">
    <PageHeader title="Tags">
      <template #actions>
        <PrimeButton
          label="Add new tag"
          icon="pi pi-plus"
          @click="openEditDialog()"
        />
      </template>
    </PageHeader>

    <TagsList
      :tags="tags"
      :loading="queryPending"
      @edit="openEditDialog"
      @delete="setTagForDelete"
    />

    <EditTag
      v-if="showEditDialog"
      :tag="tagForEdit"
      @close="closeEditDialog"
    />
    <DeleteTag
      v-if="tagForDelete != null"
      :tag="tagForDelete"
      @close="closeDeleteDialog"
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
import PageHeader from '@/components/common/PageHeader.vue';

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
