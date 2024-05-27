<template>
  <div class="header">
    <slot name="left" />
    <slot name="right" />
  </div>
</template>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.header {
  @include flex-row-justify-between-wrapping;
  margin-bottom: var(--default-padding);
}
</style>
