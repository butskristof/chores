<template>
  <div class="layout-footer">
    <div class="logo-author">
      <img
        :src="logoUrl"
        alt="Logo"
        height="20"
        class="mr-2"
      />
      <span class="app-name">Chores</span>
      by
      <span class="author">Kristof Buts</span>
    </div>
  </div>
</template>

<script setup>
import { useLayout } from '@/layout/app/composables/layout.js';
import { computed } from 'vue';

const { layoutConfig } = useLayout();

const logoUrl = computed(() => {
  return `layout/images/${layoutConfig.darkTheme.value ? 'chores-white' : 'chores-dark'}.svg`;
});
</script>

<style lang="scss" scoped>
.layout-footer {
  justify-content: flex-end;
  gap: 0.5rem;

  .logo-author {
    img {
      display: inline-block;
      vertical-align: middle;
      margin-right: 0.25rem;
    }

    .app-name,
    .author {
      font-weight: 500;
    }
  }
}
</style>
