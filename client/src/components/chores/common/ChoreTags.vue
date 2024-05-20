<template>
  <div
    v-if="tags.length > 0"
    class="tags"
  >
    <AppTag
      v-for="tag in tags"
      :key="tag.id"
      :tag="tag"
    />
  </div>
</template>

<script setup>
import AppTag from '@/components/tags/common/AppTag.vue';

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
