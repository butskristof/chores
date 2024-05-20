<template>
  <div
    class="header"
    :class="{ 'inline-padding': inlinePadding }"
  >
    <slot name="title">
      <h1>{{ title }}</h1>
    </slot>
    <div class="actions">
      <slot name="actions"></slot>
    </div>
  </div>
</template>

<script setup>
defineProps({
  title: {
    type: String,
    default: () => null,
  },
  inlinePadding: {
    type: Boolean,
    default: true,
  },
});
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.header {
  @include flex-row-justify-between-wrapping;
  margin-bottom: var(--default-padding);

  &.inline-padding {
    padding-inline: var(--default-padding);
  }
}
</style>
