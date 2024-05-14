<template>
  <div class="chore-detail">
    <DebugValue :value="chore" />
  </div>
</template>

<script setup>
import DebugValue from '@/components/debug/DebugValue.vue';
import { useRouteParams } from '@vueuse/router';
import { useChoresApiChore } from '@/composables/queries/chores-api.js';
import { computed } from 'vue';

const choreId = useRouteParams('id');
const choreQuery = useChoresApiChore(choreId);
const chore = computed(() => choreQuery.data.value);
</script>

<style scoped lang="scss"></style>
