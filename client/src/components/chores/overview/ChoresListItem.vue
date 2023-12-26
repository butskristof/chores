<template>
  <div class="chores-list-item">
    <div class="row">
      <div class="title">
        <router-link :to="{ name: routes.chores.children.detail.name, params: { id: chore.id } }">
          {{ chore.name }}
        </router-link>
      </div>
      <div class="tags">
        {{ chore.tags.map((t) => t.name).join(', ') }}
      </div>
    </div>
    <ChoreLastIteration :chore="chore" />
  </div>
</template>

<script setup>
import { routes } from '@/router/routes';
import ChoreLastIteration from '@/components/chores/common/ChoreLastIteration.vue';

defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped lang="scss">
.chores-list-item {
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;

  .row {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
  }

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
