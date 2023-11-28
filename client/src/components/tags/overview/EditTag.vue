<template>
  <Dialog
    :open="open"
    @close="emit('close')"
  >
    <DialogPanel>
      <DialogTitle>{{ title }}</DialogTitle>
      <div>hey</div>
      <div v-if="props.tag != null">
        <pre>{{ JSON.stringify(props.tag, null, 2) }}</pre>
      </div>
    </DialogPanel>
  </Dialog>
</template>

<script setup>
import { computed } from 'vue';
import { Dialog, DialogPanel, DialogTitle } from '@headlessui/vue';

const props = defineProps({
  open: {
    type: Boolean,
    default: false,
  },
  tag: {
    type: Object,
    default: null,
  },
});
const emit = defineEmits(['close']);

const isEdit = computed(() => props.tag != null);
const title = computed(() => (isEdit.value === true ? 'Edit tag' : 'Add a new tag'));
</script>

<style scoped lang="scss"></style>
