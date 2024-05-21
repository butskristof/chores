<template>
  <PrimeMessage
    :severity
    :closable="false"
    >{{ message }}</PrimeMessage
  >
</template>

<script setup>
import { computed } from 'vue';
import { CHORE_DUE_STATES, getChoreDueState, getDueDays } from '@/utilities/chores.js';
import PrimeMessage from 'primevue/message';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

const dueDays = computed(() => getDueDays(props.chore));
const state = computed(() => getChoreDueState(props.chore));
const SEVERITIES = {
  [CHORE_DUE_STATES.OK]: 'success',
  [CHORE_DUE_STATES.ALMOST_DUE]: 'warn',
  [CHORE_DUE_STATES.OVERDUE]: 'error',
};
const severity = computed(() => SEVERITIES[state.value]);
const message = computed(() => {
  if (dueDays.value < 0) return `due ${dueDays.value} ago`;
  else if (dueDays.value === 1) return `due tomorrow`;
  else if (dueDays.value === 0) return `due today`;
  return `due in ${dueDays.value} days`;
});
</script>

<style scoped lang="scss"></style>
