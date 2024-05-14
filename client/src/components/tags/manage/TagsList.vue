<template>
  <div class="tags-list">
    <ul v-if="loading">
      <li
        v-for="i in 3"
        :key="i"
        class="skeleton"
      >
        <div class="details">
          <PrimeSkeleton
            width="10rem"
            height="1.6rem"
          />
          <PrimeSkeleton
            width="6rem"
            height="0.8rem"
          />
        </div>
        <div class="actions">
          <PrimeSkeleton
            width="2.5rem"
            height="calc(2rem + 2px)"
          />
          <PrimeSkeleton
            width="2.5rem"
            height="calc(2rem + 2px)"
          />
        </div>
      </li>
    </ul>
    <ul v-else-if="tags.length > 0">
      <li
        v-for="tag in tags"
        :key="tag.id"
      >
        <div class="details">
          <div class="name">{{ tag.name }}</div>
          <div class="usage">used by {{ tag.choresCount }} chores</div>
        </div>
        <div class="actions">
          <PrimeButton
            icon="pi pi-pencil"
            @click="$emit('edit', tag.id)"
          />
          <PrimeButton
            icon="pi pi-trash"
            severity="danger"
            @click="$emit('delete', tag.id)"
          />
        </div>
      </li>
    </ul>
    <div
      v-else
      class="empty-state"
    >
      No tags found, get started by adding a new one.
    </div>
  </div>
</template>

<script setup>
import PrimeButton from 'primevue/button';
import PrimeSkeleton from 'primevue/skeleton';

defineProps({
  tags: {
    type: Array,
    required: true,
  },
  loading: {
    type: Boolean,
    default: false,
  },
});
defineEmits(['edit', 'delete']);
</script>

<style scoped lang="scss">
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';

.tags-list {
  @include white-content-box;

  &:has(ul) {
    padding-block: 0;
  }

  ul {
    list-style: none;
    padding: 0;
    margin: 0;

    li {
      &.skeleton .details {
        @include flex-column;
        gap: 0.5rem;
      }
      &:not(:last-of-type) {
        border-bottom: 1px solid var(--surface-border);
      }
      padding-block: 1rem;
      @include flex-row-justify-between-wrapping;
      align-items: flex-start;

      .details {
        .name {
          font-size: 120%;
        }
        .usage {
          color: var(--text-color-secondary);
          font-size: 80%;
        }
      }

      .actions {
        @include flex-row;
        align-items: center;
        gap: 0.5rem;
      }
    }
  }
}
</style>
