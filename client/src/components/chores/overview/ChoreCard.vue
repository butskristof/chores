<template>
  <Card style="width: 25rem">
    <template #title>{{ chore.name }}</template>
    <template #content>
      <ChoreLastIteration :chore="chore" />
      <div class="tags-actions">
        <div>
          <div class="tags">
            <Tag
              v-for="tag in chore.tags"
              :key="tag.id"
              :value="tag.name"
            />
          </div>
        </div>
        <div class="actions">
          <router-link :to="{ name: routes.chores.children.detail.name, params: { id: chore.id } }">
            <Button
              icon="pi pi-chevron-right"
              icon-pos="right"
            />
          </router-link>
        </div>
      </div>
    </template>
  </Card>
</template>

<script setup>
import ChoreLastIteration from '@/components/chores/common/ChoreLastIteration.vue';
import { routes } from '@/router/routes.js';

defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped lang="scss">
.tags-actions {
  margin-top: 1rem;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}
.tags {
  margin-top: 0.5rem;
  display: flex;
  flex-wrap: wrap;
  //justify-content: flex-end;
  gap: 0.5rem;
}
</style>
