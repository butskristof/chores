<template>
  <div class="chores-overview">
    <PageHeader title="Chores">
      <template #actions>
        <PrimeButton
          label="Add new chore"
          icon="pi pi-plus"
          @click="showCreate = true"
        />
      </template>
    </PageHeader>

    <ChoresList :chores="chores" />

    <EditChore
      v-if="showCreate"
      @close="showCreate = false"
    />
  </div>
</template>

<script setup>
import PageHeader from '@/components/common/PageHeader.vue';
import PrimeButton from 'primevue/button';
import { computed, ref } from 'vue';
import EditChore from '@/components/chores/detail/EditChore.vue';
import ChoresList from '@/components/chores/overview/ChoresList.vue';
import { useChoresApiChores } from '@/composables/queries/chores-api.js';

const choresQuery = useChoresApiChores();
const chores = computed(() => choresQuery.data.value?.chores ?? []);

//#region create
const showCreate = ref(false);
//#endregion
</script>

<style scoped lang="scss"></style>
