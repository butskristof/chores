<template>
  <div class="header">
    <h1>Tags</h1>
    <div class="actions">
      <PrimeButton
        label="New"
        icon="pi pi-plus"
      />
    </div>
  </div>
  <TagsList />
</template>

<script setup>
import TagsList from '@/components/tags/manage/TagsList.vue';
</script>

<style scoped lang="scss">
@import '@/styles/_layout-elements.scss';

.header {
  @include header-space-between;
}
</style>
