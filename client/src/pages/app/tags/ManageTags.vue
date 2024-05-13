<template>
  <div class="header">
    <h1>Tags</h1>
    <div class="actions">
      <PrimeButton
        label="New"
        icon="pi pi-plus"
      />
    </div>
  </div>
  <TagsList :tags="tags" />
</template>

<script setup>
import TagsList from '@/components/tags/manage/TagsList.vue';
import { useChoresApiTags } from '@/composables/queries/chores-api.js';
import { computed } from 'vue';

const tagsQuery = useChoresApiTags();
const tags = computed(() => tagsQuery.data.value?.tags ?? []);
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.header {
  @include flex-row-justify-between-wrapping;
  margin-bottom: var(--default-padding);
}
</style>
