<template>
  <PrimeDialog
    :visible="true"
    modal
    maximizable
    :draggable="false"
    :header="isEdit ? 'Edit tag' : 'Create new tag'"
    :style="{ width: '50rem' }"
    :breakpoints="{ '1199px': '75vw', '575px': '95vw' }"
    @update:visible="updateVisible"
  >
    <div>form goes here</div>
  </PrimeDialog>
</template>

<script setup>
import { computed } from 'vue';
import PrimeDialog from 'primevue/dialog';

const props = defineProps({
  tag: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);
const isEdit = computed(() => props.tag != null);

//#region create/update
//#endregion

const updateVisible = (value) => {
  if (value === false) tryClose();
};
const tryClose = () => {
  emit('close');
};
</script>

<style scoped lang="scss"></style>
