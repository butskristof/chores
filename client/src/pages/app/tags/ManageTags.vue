<template>
  <div class="header">
    <h1>Tags</h1>
    <div class="actions">
      <PrimeButton
        label="New"
        icon="pi pi-plus"
        @click="openEditDialog()"
      />
    </div>
  </div>

  <TagsList
    :tags="tags"
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
</template>

<script setup>
import TagsList from '@/components/tags/manage/TagsList.vue';
import { useChoresApiTags } from '@/composables/queries/chores-api.js';
import { computed, ref } from 'vue';
import EditTag from '@/components/tags/manage/EditTag.vue';
import DeleteTag from '@/components/tags/manage/DeleteTag.vue';
import PrimeButton from 'primevue/button';

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
const closeDeleteDialog = () => (tagForDelete.value = null);

//#endregion
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.header {
  @include flex-row-justify-between-wrapping;
  margin-bottom: var(--default-padding);
  padding-inline: var(--default-padding);
}
</style>
