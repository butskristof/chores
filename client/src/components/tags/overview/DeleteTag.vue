<template>
  <Dialog
    :open="open"
    @close="emit('close')"
  >
    <DialogPanel>
      <DialogTitle>Delete tag</DialogTitle>
      <div>hey</div>
    </DialogPanel>
  </Dialog>
</template>

<script setup>
import { Dialog, DialogPanel, DialogTitle } from '@headlessui/vue';
defineProps({
  open: {
    type: Boolean,
    default: false,
  },
});
const emit = defineEmits(['close']);
</script>

<style scoped lang="scss"></style>
