<template>
  <div class="header">
    <h2>Chores</h2>
    <div class="actions">
      <button
        type="button"
        @click="showCreateDialog = true"
      >
        create new
      </button>
    </div>
  </div>

  <EditChore
    v-if="showCreateDialog"
    :open="true"
    @close="showCreateDialog = false"
  />

  <QueryData
    v-slot="{ data }"
    :query="choresQuery"
  >
    <ChoresList :chores="data.value.chores" />
  </QueryData>
</template>

<script setup>
import ChoresList from '@/components/chores/overview/ChoresList.vue';
import { useChoresApiChores } from '@/composables/queries/chores-api';
import QueryData from '@/components/common/QueryData.vue';
import { ref } from 'vue';
import EditChore from '@/components/chores/common/EditChore.vue';

const choresQuery = useChoresApiChores();

//#region create
const showCreateDialog = ref(false);
//#endregion
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
