<template>
  <div
    v-if="tags.length > 0"
    class="tags"
  >
    <ChoreTag
      v-for="tag in tags"
      :key="tag.id"
      :tag
    />
  </div>
</template>

<script setup>
import ChoreTag from '@/components/tags/common/ChoreTag.vue';

defineProps({
  tags: {
    type: Array,
    default: () => [],
  },
});
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.tags {
  @include flex-row;
  flex-wrap: wrap;
  gap: 0.5rem;
}
</style>
