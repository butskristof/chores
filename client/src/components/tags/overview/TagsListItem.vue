<template>
  <div class="tags-list-item">
    <div class="left">
      <div>{{ tag.name }}</div>
    </div>
    <div class="right">
      <button type="button">edit</button>
      <button type="button">delete</button>
    </div>
  </div>
</template>

<script setup>
defineProps({
  tag: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped lang="scss">
.tags-list-item {
  display: flex;
  flex-direction: row;
  justify-content: space-between;

  &:not(:last-of-type) {
    border-bottom: 1px solid gray;
  }

  .right {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
  }
}
</style>
