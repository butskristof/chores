<template>
  <div class="home">
    <div class="chores">
      <ChoreListItem />
      <ChoreListItem />
      <ChoreListItem />
    </div>
  </div>
</template>

<script setup>
import ChoreListItem from '@/pages/ChoreListItem.vue';
</script>

<style scoped lang="scss">
.chores {
  > * {
    margin-bottom: 1rem;
  }
}
</style>
