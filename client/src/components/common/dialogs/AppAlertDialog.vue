<template>
  <Dialog
    class="dialog"
    :open="open"
    @close="emit('close')"
  >
    <div
      class="backdrop"
      aria-hidden="true"
    />

    <div class="dialog-scroll-container">
      <div class="dialog-panel-container">
        <DialogPanel class="dialog-panel">
          <DialogTitle v-if="$slots.title">
            <slot name="title"></slot>
          </DialogTitle>

          <DialogDescription v-if="$slots.description">
            <slot name="description"></slot>
          </DialogDescription>

          <slot></slot>
        </DialogPanel>
      </div>
    </div>
  </Dialog>
</template>

<script setup>
import { Dialog, DialogDescription, DialogPanel, DialogTitle } from '@headlessui/vue';
defineProps({
  open: {
    type: Boolean,
    default: false,
  },
});
const emit = defineEmits(['close']);
</script>

<style scoped lang="scss">
.dialog {
  position: relative;
  z-index: 50;
}

.backdrop {
  position: fixed;
  inset: 0;
  background-color: rgb(0, 0, 0, 0.33);
}

// full-screen scrollable container
.dialog-scroll-container {
  position: fixed;
  inset: 0;
  width: 100vw;
  overflow-y: auto;

  // container to center the panel
  .dialog-panel-container {
    display: flex;
    min-height: 100%;
    align-items: center;
    justify-content: center;
    padding: 1rem;
  }
}

.dialog-panel {
  width: 100%;
  max-width: 24rem;
  background-color: white;
  padding: 1rem;

  h2 {
    margin: auto;
  }
}
</style>
