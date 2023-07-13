<template>
  <header>
    <div class="brand">
      <ListTodo />
      <span>Chores</span>
    </div>
    <div class="actions">Log in</div>
  </header>
</template>

<script setup>
import { ListTodo } from 'lucide-vue-next';
</script>

<style scoped lang="scss">
@import '@/styles/_variables.scss';

header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;

  margin-bottom: $box-padding;
  padding-top: $box-padding;
  padding-bottom: $box-padding;
  background-color: var(--hiq-color-gray-7);

  .brand {
    display: flex;
    flex-direction: row;
    align-items: center;
    font-weight: 500;
    font-size: 120%;

    :first-child {
      display: block;
      margin-right: 0.5rem;
    }
  }
}
</style>
