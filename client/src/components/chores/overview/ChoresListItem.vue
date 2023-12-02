<template>
  <div class="chores-list-item">
    <div class="title">{{ chore.title }}</div>
    <div class="due">due in {{ due }} days</div>
  </div>
</template>

<script setup>
defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
const due = Math.floor(Math.random() * 10) - 1;
</script>

<style scoped lang="scss">
.chores-list-item {
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;

  &:nth-of-type(even) {
    background-color: rgb(248, 248, 248);
  }

  &:hover {
    background-color: rgb(238, 238, 238);
  }

  &:not(:last-of-type) {
    border-bottom: 1px solid gray;
  }
}
</style>
