<template>
  <template v-if="query.isLoading.value === true">
    <LoadingIndicator />
  </template>
  <template v-else-if="query.isError.value === true">
    <div class="query-error">
      <p>{{ query.error }}</p>
    </div>
  </template>
  <slot
    v-else
    :data="query.data"
  >
    <div>
      <pre>{{ JSON.stringify(query.data.value, null, 2) }}</pre>
    </div>
  </slot>
</template>

<script setup>
import LoadingIndicator from '@/components/common/LoadingIndicator.vue';

defineProps({
  query: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped lang="scss"></style>
