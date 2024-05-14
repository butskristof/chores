<template>
  <div class="chores-list">
    <ul v-if="chores.length > 0">
      <ChoreListItem
        v-for="(chore, i) in chores"
        :key="chore.id"
        :chore="chore"
        :index="i"
      />
    </ul>
  </div>
</template>

<script setup>
import ChoreListItem from '@/components/chores/overview/ChoreListItem.vue';

defineProps({
  chores: {
    type: Array,
    required: true,
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
    list-style: none;
    padding: 0;
    margin: 0;

    @include flex-column;
    gap: 1rem;
  }
}
</style>
