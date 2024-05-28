<template>
  <PrimeTag
    class="tag"
    :class="{ color: hasColor }"
    severity="secondary"
  >
    <AppIcon
      v-if="!stringIsNullOrWhitespace(tag.icon)"
      :name="tag.icon"
    />
    <span class="name">{{ tag.name }}</span>
  </PrimeTag>
</template>

<script setup>
import PrimeTag from 'primevue/tag';
import { stringIsNullOrWhitespace } from '@/utilities/string.js';
import AppIcon from '@/components/common/AppIcon.vue';
import { computed } from 'vue';
import { getTextColorForBackground } from '@/utilities/color.js';

const props = defineProps({
  tag: {
    type: Object,
    required: true,
  },
});

const color = computed(() => props.tag.color);
const hasColor = computed(() => !stringIsNullOrWhitespace(color.value));
const textColor = computed(() => (hasColor.value ? getTextColorForBackground(color.value) : null));
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.tag {
  border: 1px solid;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding-inline: 0.5rem;

  &.color {
    background-color: v-bind('tag.color');
    color: v-bind('textColor');
    border: none;
  }
}
</style>
