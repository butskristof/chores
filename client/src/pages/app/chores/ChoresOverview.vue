<template>
  <div class="card">
    <Toolbar class="header">
      <template #start>
        <h1>Chores</h1>
      </template>
      <template #end>
        <Button
          label="New"
          icon="pi pi-plus"
          class="p-button-success"
          @click="showCreateDialog = true"
        />
      </template>
    </Toolbar>

    <ChoresList :chores="chores" />
  </div>

  <EditChore
    v-if="showCreateDialog"
    :open="true"
    @close="showCreateDialog = false"
  />
</template>

<script setup>
import ChoresList from '@/components/chores/overview/ChoresList.vue';
import { useChoresApiChores } from '@/composables/queries/chores-api.js';
import { computed, ref } from 'vue';
import EditChore from '@/components/chores/common/EditChore.vue';
import Toolbar from 'primevue/toolbar';
import Button from 'primevue/button';

const choresQuery = useChoresApiChores();
const chores = computed(() => choresQuery.data.value?.chores ?? []);

//#region create

const showCreateDialog = ref(false);

//#endregion
</script>

<style scoped lang="scss">
.header {
  margin-bottom: 2rem;

  h1 {
    font-size: 1.75rem;
    margin-block: 0;
  }
}
</style>
