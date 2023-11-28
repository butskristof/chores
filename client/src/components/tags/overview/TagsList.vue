<template>
  <div class="header">
    <div class="title">
      <h2>Tags</h2>
    </div>
    <div class="actions">
      <button type="button">+</button>
    </div>
  </div>

  <div class="tags-list">
    <QueryData
      v-slot="{ data }"
      :query="tagsQuery"
    >
      <TagsListItem
        v-for="tag in data.value"
        :key="tag.id"
        :tag="tag"
      />
    </QueryData>
  </div>
</template>

<script setup>
import { useChoresApiTags } from '@/composables/queries/chores-api';
import QueryData from '@/components/common/QueryData.vue';
import TagsListItem from '@/components/tags/overview/TagsListItem.vue';

const tagsQuery = useChoresApiTags();
</script>

<style scoped lang="scss">
.header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
}
</style>
