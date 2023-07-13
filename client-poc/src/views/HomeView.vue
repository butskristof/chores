<template>
  <div class="home">
    <div class="chores">
      <ChoreListItem v-for="chore in CHORES" :key="chore.id" :chore="chore" />
    </div>
  </div>
</template>

<script setup>
import ChoreListItem from '@/pages/ChoreListItem.vue';
import { CHORES } from '@/data/chores';
</script>

<style scoped lang="scss">
.chores {
  > * {
    margin-bottom: 1rem;
  }
}
</style>
