<template>
  <div class="tags-list-item">
    <div class="left">
      <div class="name">{{ tag.name }}</div>
      <div class="usage">
        Used by
        <router-link to="#"> {{ tag.choresCount }} chores </router-link>
      </div>
    </div>
    <div class="right">
      <button
        type="button"
        @click="emit('edit')"
      >
        edit
      </button>
      <Tippy
        :content="canDelete ? null : 'Tags which are in use cannot be deleted'"
        placement="bottom-end"
      >
        <button
          type="button"
          :disabled="!canDelete"
          @click="emit('delete')"
        >
          delete
        </button>
      </Tippy>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import { Tippy } from 'vue-tippy';

const props = defineProps({
  tag: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(['edit', 'delete']);

// const canDelete = computed(() => props.tag.choresCount != null && props === 0);
const canDelete = computed(() => props.tag != null);
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

  .left {
    .usage {
      font-size: 80%;
    }
  }

  .right {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
    align-items: center;
    justify-content: center;
  }
}
</style>
