<template>
  <div class="filtering"></div>
  <div class="list">
    <ChoreListItem v-for="chore in CHORES" :key="chore.id" :chore="chore" />
  </div>
</template>

<script setup>
import ChoreListItem from '@/components/chores/ChoreListItem.vue';
import { CHORES } from '@/data/chores';
</script>

<style scoped lang="scss">
.list {
  > * {
    margin-bottom: 1rem;
  }
}
</style>
