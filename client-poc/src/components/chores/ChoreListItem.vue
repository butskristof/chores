<template>
  <div class="chore-list-item">
    <div class="header">
      <div class="title is-large">{{ chore.title }}</div>
      <router-link
        :to="{
          name: ROUTES.chores.children.detail.name,
          params: { id: chore.id },
        }"
        class="button btn-reset detail-link"
      >
        <ChevronRight />
      </router-link>
    </div>
    <div class="iterations">
      <div class="last">
        Last iteration: {{ chore.lastIteration.toLocaleDateString() }}
      </div>
      <div class="next">
        Next iteration: {{ chore.nextIteration.toLocaleDateString() }}
      </div>
    </div>
  </div>
</template>

<script setup>
import { ChevronRight } from 'lucide-vue-next';
import { ROUTES } from '@/router/routes';
defineProps({
  chore: { type: Object, required: true },
});
</script>

<style scoped lang="scss">
@import '@/styles/variables';

.chore-list-item {
  padding: $box-padding;
  border: 1px solid var(--hiq-color-gray-6);
  border-radius: var(--hiq-border-radius);
  box-shadow: $box-shadow;

  .header {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;

    margin-bottom: $box-padding;
  }
}

.detail-link {
  padding: 0;
}
</style>
