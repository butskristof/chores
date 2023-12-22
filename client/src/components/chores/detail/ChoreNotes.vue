<template>
  <div class="header">
    <h2>Notes</h2>
    <div class="actions">
      <button
        v-if="!isEdit"
        type="button"
        @click="isEdit = true"
      >
        edit
      </button>
    </div>
  </div>

  <div class="read-view">
    <div>{{ chore.notes }}</div>
  </div>
  <div
    v-if="isEdit"
    class="edit-view"
  >
    <textarea></textarea>
    <div class="actions">
      <button
        type="submit"
        @click="isEdit = false"
      >
        Save
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';

defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

const isEdit = ref(false);
</script>

<style scoped lang="scss">
.header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;

  .actions {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
    align-items: flex-start;
  }
}
</style>
