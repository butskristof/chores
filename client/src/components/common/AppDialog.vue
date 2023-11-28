<template>
  <Dialog
    class="dialog"
    :open="open"
    @close="emit('close')"
  >
    <div
      class="backdrop"
      aria-hidden="true"
    ></div>

    <div class="dialog-scroll-container">
      <div class="dialog-panel-container">
        <DialogPanel class="dialog-panel">
          <div class="header">
            <div class="title">
              <DialogTitle>
                <slot name="title"></slot>
              </DialogTitle>
            </div>
            <div class="close">
              <button
                type="button"
                @click="emit('close')"
              >
                x
              </button>
            </div>
          </div>
          <slot name="description"></slot>
          <slot></slot>
        </DialogPanel>
      </div>
    </div>
  </Dialog>
</template>

<script setup>
import { Dialog, DialogPanel, DialogTitle } from '@headlessui/vue';

defineProps({
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
  }
}

.dialog-panel {
  width: 100%;
  max-width: 32rem;
  background-color: white;
  padding: 1rem;
  margin: 1rem;

  .header {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: flex-start;

    h2 {
      margin: auto;
    }
  }
}
</style>
