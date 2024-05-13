<template>
  <div class="tags-list">
    <ul>
      <li
        v-for="tag in tags"
        :key="tag.id"
      >
        <div class="details">
          <div class="name">{{ tag.name }}</div>
          <div class="usage">used by {{ tag.choresCount }} chores</div>
        </div>
        <div class="actions">
          <PrimeButton icon="pi pi-pencil" />
          <PrimeButton
            icon="pi pi-trash"
            severity="danger"
          />
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup>
defineProps({
  tags: {
    type: Array,
    required: true,
  },
});
</script>

<style scoped lang="scss">
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';

.tags-list {
  @include white-content-box;
  padding-block: 0;

  ul {
    list-style: none;
    padding: 0;
    margin: 0;

    li {
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
        gap: 0.5rem;
      }
    }
  }
}
</style>
