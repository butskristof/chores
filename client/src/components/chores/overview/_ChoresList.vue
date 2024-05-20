<template>
  <div class="chores-list">
    <div
      v-for="chore in sorted"
      :key="chore.id"
      class="chore"
    >
      <div class="details-tags">
        <div class="name">{{ chore.name }}</div>
        <div class="tags">
          <Tag
            v-for="tag in chore.tags"
            :key="tag.id"
            :value="tag.name"
          />
        </div>
      </div>
      <div class="due-nav">
        <ChoreLastIteration :chore="chore" />
        <div class="actions">
          <router-link :to="{ name: routes.chores.children.detail.name, params: { id: chore.id } }">
            <Button
              class="p-button-outlined"
              icon="pi pi-chevron-right"
              icon-pos="right"
            />
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { getDueDays } from '@/utilities/chores.js';
import { computed } from 'vue';
import { routes } from '@/router/routes.js';
import ChoreLastIteration from '@/components/chores/common/_ChoreLastIteration.vue';

const props = defineProps({
  chores: {
    type: Array,
    default: () => [],
  },
});

const sorted = computed(() => props.chores.toSorted((a, b) => getDueDays(a) - getDueDays(b)));
</script>

<style scoped lang="scss">
@import '@/styles/utilities';

.chore {
  padding-block: 1rem;
  @include flex-column;
  border: solid #e5e7eb;
  border-width: 0 0 1px 0;
  gap: 1rem;

  .details-tags {
    @include flex-row-justify-between;
    flex-wrap: wrap;
    gap: 1rem;

    .name {
      font-size: 1.5rem;
      font-weight: 500;
    }

    .tags {
      @include flex-row;
      gap: 0.5rem;
      margin-left: auto;
    }
  }
  .due-nav {
    @include flex-row-justify-between;
    flex-wrap: wrap;
  }
}
</style>
