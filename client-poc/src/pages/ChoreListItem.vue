<template>
  <div class="chore-list-item">
    <div class="header">
      <div class="title is-large">Chore</div>
    </div>
    <div class="description">Some description</div>
  </div>
</template>

<script setup></script>

<style scoped lang="scss">
@import '@/styles/_variables.scss';

.chore-list-item {
  padding: $box-padding;
  border: 1px solid black;
  border-radius: var(--hiq-border-radius);
}
</style>
