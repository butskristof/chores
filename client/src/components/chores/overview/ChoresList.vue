<template>
  <div class="chores-list">
    <ChoreCard
      v-for="chore in sorted"
      :key="chore.id"
      :chore="chore"
    />
  </div>
</template>

<script setup>
import { getDueDays } from '@/utilities/chores.js';
import { computed } from 'vue';
import ChoreCard from '@/components/chores/overview/ChoreCard.vue';

const props = defineProps({
  chores: {
    type: Array,
    default: () => [],
  },
});

const sorted = computed(() => props.chores.toSorted((a, b) => getDueDays(a) - getDueDays(b)));
</script>

<style scoped lang="scss">
.chores-list {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  gap: 1rem;
  justify-content: flex-start;
}
</style>
