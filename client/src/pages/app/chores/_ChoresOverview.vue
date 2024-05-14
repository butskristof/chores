<template>
  <div class="card">
    <CardHeader>
      <template #left>
        <h1>Chores</h1>
      </template>

      <template #right>
        <div class="actions">
          <Button
            label="New"
            icon="pi pi-plus"
            @click="showCreateDialog = true"
          />

          <EditChore
            v-if="showCreateDialog"
            @close="showCreateDialog = false"
          />
        </div>
      </template>
    </CardHeader>

    <ChoresList :chores="chores" />
  </div>
</template>

<script setup>
import ChoresList from '@/components/chores/overview/_ChoresList.vue';
import { useChoresApiChores } from '@/composables/queries/chores-api.js';
import { computed, ref } from 'vue';
import EditChore from '@/components/chores/common/_EditChore.vue';
import CardHeader from '@/components/common/CardHeader.vue';

const choresQuery = useChoresApiChores();
const chores = computed(() => choresQuery.data.value?.chores ?? []);

//#region create

const showCreateDialog = ref(false);

//#endregion
</script>
