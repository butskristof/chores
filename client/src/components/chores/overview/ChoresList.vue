<template>
  <div class="chores-list">
    <ChoresListItem
      v-for="chore in sorted"
      :key="chore.id"
      :chore="chore"
    />
  </div>
</template>

<script setup>
import ChoresListItem from '@/components/chores/overview/ChoresListItem.vue';
import { getDueDays } from '@/utilities/chores.js';
import { computed } from 'vue';

const props = defineProps({
  chores: {
    type: Array,
    default: () => [],
  },
});

const sorted = computed(() => props.chores.toSorted((a, b) => getDueDays(a) - getDueDays(b)));
</script>

<style scoped lang="scss"></style>
