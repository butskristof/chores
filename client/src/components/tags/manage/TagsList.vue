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
            height="2rem"
          />
          <PrimeSkeleton
            width="6rem"
            height="1rem"
          />
        </div>
        <div class="actions">
          <PrimeSkeleton
            v-for="j in 2"
            :key="j"
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
          <AppTag
            :tag="tag"
            class="tag"
          />
          <div class="usage">used by {{ tag.choresCount }} chores</div>
        </div>
        <div class="actions">
          <PrimeButton
            aria-label="Edit tag"
            icon="pi pi-pencil"
            @click="$emit('edit', tag.id)"
          />
          <Tippy
            :content="tag.choresCount > 0 ? 'Tag cannot be deleted if it\'s in use' : null"
            placement="top-end"
          >
            <PrimeButton
              aria-label="Delete tag"
              icon="pi pi-trash"
              severity="danger"
              :disabled="tag.choresCount > 0"
              @click="$emit('delete', tag.id)"
            />
          </Tippy>
        </div>
      </li>
    </ul>
    <div v-else>No tags found, get started by adding a new one.</div>
  </div>
</template>

<script setup>
import PrimeButton from 'primevue/button';
import PrimeSkeleton from 'primevue/skeleton';
import AppTag from '@/components/tags/common/AppTag.vue';
import { Tippy } from 'vue-tippy';

defineProps({
  tags: {
    type: Array,
    default: () => [],
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
    @include unstyled-ul;
    li {
      &.skeleton .details {
        @include flex-column;
        gap: 0.125rem;
      }
      &:not(:last-of-type) {
        border-bottom: 1px solid var(--border-color);
      }
      padding-block: 1rem;

      @include flex-row-justify-between-wrapping;
      align-items: flex-start;

      .details {
        .usage {
          color: var(--text-color-secondary);
          font-size: 80%;
        }
      }

      .actions {
        @include flex-row-actions;
      }
    }
  }
}
</style>
