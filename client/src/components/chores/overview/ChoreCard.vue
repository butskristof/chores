<template>
  <Card style="width: 25rem">
    <template #title>{{ chore.name }}</template>
    <template #content>
      <ChoreLastIteration :chore="chore" />
      <div class="tags">
        <Tag
          v-for="tag in chore.tags"
          :key="tag.id"
          :value="tag.name"
        />
      </div>
    </template>
  </Card>
</template>

<script setup>
import Card from 'primevue/card';
import Tag from 'primevue/tag';
import ChoreLastIteration from '@/components/chores/common/ChoreLastIteration.vue';

defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped lang="scss">
.tags {
  margin-top: 1rem;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: flex-end;
  gap: 0.5rem;
}
</style>
