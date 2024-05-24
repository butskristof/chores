<template>
  <PrimeDialog
    v-bind="$attrs"
    visible
    modal
    maximizable
    :draggable="false"
    :style="{
      width: '50rem',
    }"
    @update:visible="updateVisible"
  >
    <slot />
  </PrimeDialog>
</template>

<script setup>
import PrimeDialog from 'primevue/dialog';

const emit = defineEmits(['close']);

const updateVisible = (value) => {
  if (value === false) emit('close');
};
</script>

<style scoped lang="scss"></style>
