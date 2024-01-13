<template>
  <div class="header">
    <slot name="left"></slot>
    <slot name="right"></slot>
  </div>
</template>

<script setup></script>

<style scoped lang="scss">
@import '@/styles/utilities';

.header {
  @include flex-row-justify-between;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 1rem;

  :slotted(h1) {
    font-size: 2rem;
    margin-block: 0;
  }

  :slotted(.actions) {
    @include flex-row;
    gap: 0.5rem;
    justify-content: flex-end;
    margin-left: auto;
  }
}
</style>
