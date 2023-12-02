<template>
  <div class="chores-list">
    <ChoresListItem
      v-for="chore in chores"
      :key="chore.id"
      :chore="chore"
    />
  </div>
</template>

<script setup>
import ChoresListItem from '@/components/chores/overview/ChoresListItem.vue';

defineProps({
  chores: {
    type: Array,
    default: () => [],
  },
});
</script>

<style scoped lang="scss"></style>
