<template>
  <div class="header">
    <h1>{{ title }}</h1>
    <div class="actions">
      <slot name="actions"></slot>
    </div>
  </div>
</template>

<script setup>
defineProps({
  title: {
    type: String,
    required: true,
  },
});
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.header {
  @include flex-row-justify-between-wrapping;
  margin-bottom: var(--default-padding);
  padding-inline: var(--default-padding);
}
</style>
