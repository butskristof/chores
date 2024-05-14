<template>
  <PrimeTag class="tag">
    <i
      v-if="!stringIsNullOrWhitespace(tag.icon)"
      :class="tag.icon"
    />
    <span class="name">{{ tag.name }}</span>
  </PrimeTag>
</template>

<script setup>
import PrimeTag from 'primevue/tag';
import { stringIsNullOrWhitespace } from '@/utilities/string.js';

defineProps({
  tag: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.tag {
  border: 1px solid;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding-inline: 0.5rem;

  --tag-color: v-bind('tag.color');
  //background-color: var(--tag-color);
}
</style>
