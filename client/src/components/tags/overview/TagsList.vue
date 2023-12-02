<template>
  <div class="tags-list">
    <TagsListItem
      v-for="tag in tags"
      :key="tag.id"
      :tag="tag"
      @edit="emit('edit', tag.id)"
      @delete="emit('delete', tag.id)"
    />
  </div>
</template>

<script setup>
import TagsListItem from '@/components/tags/overview/TagsListItem.vue';

defineProps({
  tags: {
    type: Array,
    default: () => [],
  },
});
const emit = defineEmits(['edit', 'delete']);
</script>
