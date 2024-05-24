<template>
  <div class="chores-list">
    <template v-if="loading">
      <ChoreListItemSkeleton
        v-for="i in 3"
        :key="i"
      />
    </template>
    <ul v-else-if="chores.length > 0">
      <ChoreListItem
        v-for="chore in chores"
        :key="chore.id"
        :chore
      />
    </ul>
    <div v-else>No chores found, get started by adding a new one.</div>
  </div>
</template>

<script setup>
import ChoreListItem from '@/components/chores/overview/ChoreListItem.vue';
import ChoreListItemSkeleton from '@/components/chores/overview/ChoreListItemSkeleton.vue';

defineProps({
  chores: {
    type: Array,
    default: () => [],
  },
  loading: {
    type: Boolean,
    default: false,
  },
});
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.chores-list {
  @include white-content-box;

  ul {
    @include unstyled-ul;
    @include flex-column;
    gap: 1rem;
  }
}
</style>
