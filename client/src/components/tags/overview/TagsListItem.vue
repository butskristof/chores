<template>
  <div class="tags-list-item">
    <div class="left">
      <div class="name">{{ tag.name }}</div>
      <div class="id">{{ tag.id }}</div>
    </div>
    <div class="right">
      <button
        type="button"
        @click="emit('edit')"
      >
        edit
      </button>
      <button
        type="button"
        @click="emit('delete')"
      >
        delete
      </button>
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
const emit = defineEmits(['edit', 'delete']);
</script>

<style scoped lang="scss">
.tags-list-item {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;

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

  .right {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
  }

  .id {
    color: gray;
    font-size: 80%;
  }
}
</style>
