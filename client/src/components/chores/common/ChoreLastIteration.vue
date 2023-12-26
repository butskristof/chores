<template>
  <div v-if="lastIteration">
    Happened last {{ formatDate(lastIteration) }} - due again
    <span v-if="due === 0">today</span>
    <span v-else-if="due > 0"> in {{ due }} days</span>
    <span v-else-if="due < 0">{{ due * -1 }} days ago</span>
  </div>
</template>

<script setup>
import { formatDate } from '@/utilities/datetime.js';
import { computed } from 'vue';
import { getDueDays, getLastIteration } from '@/utilities/chores.js';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
const lastIteration = computed(() => getLastIteration(props.chore));
const due = computed(() => getDueDays(props.chore, lastIteration.value));
</script>

<style scoped lang="scss"></style>
