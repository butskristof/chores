<template>
  <div class="header">
    <h2>Chores</h2>
    <div class="actions">
      <button type="button">create new</button>
    </div>
  </div>

  <QueryData
    v-slot="{ data }"
    :query="choresQuery"
  >
    <ChoresList :chores="data.value" />
  </QueryData>
</template>

<script setup>
import ChoresList from '@/components/chores/overview/ChoresList.vue';
import { useChoresApiChores } from '@/composables/queries/chores-api';
import QueryData from '@/components/common/QueryData.vue';

const choresQuery = useChoresApiChores();
</script>

<style scoped lang="scss">
.header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;

  margin-bottom: 1rem;
}
</style>
