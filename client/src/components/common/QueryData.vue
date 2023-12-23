<template>
  <template v-if="query.isLoading.value === true">
    <LoadingIndicator />
  </template>
  <template v-else-if="query.isError.value === true">
    <div class="query-error">
      <p>Something went wrong while trying to retrieve data: {{ query.error.value?.message }}</p>
      <button
        v-if="!showFullError"
        @click="showFullError = true"
      >
        show error details
      </button>
      <pre
        v-if="showFullError"
        class="full-error"
        >{{ JSON.stringify(query.error.value, null, 2) }}</pre
      >
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
import { ref } from 'vue';

defineProps({
  query: {
    type: Object,
    required: true,
  },
});

const showFullError = ref(false);
</script>

<style scoped lang="scss"></style>
