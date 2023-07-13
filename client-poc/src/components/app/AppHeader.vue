<template>
  <header>
    <div class="brand">
      <router-link to="/" class="brand-link">
        <ListTodo class="brand-icon" />
        &nbsp;
        <span>Chores</span>
      </router-link>
    </div>
    <div class="actions">Log in</div>
  </header>
</template>

<script setup>
import { ListTodo } from 'lucide-vue-next';
</script>

<style scoped lang="scss">
@import '@/styles/_variables.scss';
@import '@/styles/_typography.scss';
@import '@/styles/_utilities.scss';

header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;

  margin-bottom: $box-padding;
  padding-top: $box-padding;
  padding-bottom: $box-padding;
  background-color: var(--hiq-color-gray-7);

  box-shadow: $box-shadow;

  .brand {
    .brand-link {
      @include link-reset;
      display: flex;
      flex-direction: row;
      align-items: center;
      font-weight: 500;
      font-size: 120%;

      .brand-icon {
        @include icon-font-size;
      }
    }
  }
}
</style>
