<template>
  <div class="chore-iterations">
    <PageHeader :inline-padding="false">
      <template #title>
        <h2>Iterations</h2>
      </template>
      <template #actions>
        <div class="actions">
          <PrimeButton
            label="Add iteration"
            icon="pi pi-plus"
            @click="openEditDialog()"
          />
        </div>
      </template>
    </PageHeader>

    <div v-if="chore.iterations.length > 0">
      <DebugValue :value="chore.iterations" />
    </div>
    <div v-else>This chore doesn't have any iterations registered.</div>

    <EditIteration
      v-if="showEditDialog"
      :iteration="iterationForEdit"
      :chore-id="chore.id"
      @close="closeEditDialog"
    />
  </div>
</template>

<script setup>
import PageHeader from '@/components/common/PageHeader.vue';
import PrimeButton from 'primevue/button';
import { ref } from 'vue';
import DebugValue from '@/components/debug/DebugValue.vue';
import EditIteration from '@/components/chores/detail/EditIteration.vue';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

//#region edit
const showEditDialog = ref(false);
const iterationForEdit = ref(null);
const openEditDialog = (id = null) => {
  iterationForEdit.value = id == null ? null : props.chore.iterations.find((i) => i.id === id);
  showEditDialog.value = true;
};
const closeEditDialog = () => {
  showEditDialog.value = false;
  iterationForEdit.value = null;
};
//#endregion
</script>

<style scoped lang="scss"></style>
