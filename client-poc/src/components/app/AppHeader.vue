<template>
  <header>
    <div class="app-name">Chores</div>
    <div class="actions">Log in</div>
  </header>
</template>

<script setup></script>

<style scoped lang="scss">
@import '@/styles/_variables.scss';

header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;

  margin-bottom: $box-padding;
}
</style>
