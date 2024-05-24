<template>
  <PrimeMessage
    :severity
    :closable="false"
    >{{ message }}</PrimeMessage
  >
</template>

<script setup>
import { computed } from 'vue';
import { CHORE_DUE_STATES, getDueDaysMessage } from '@/utilities/chores.js';
import PrimeMessage from 'primevue/message';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

const SEVERITIES = {
  [CHORE_DUE_STATES.OK]: 'success',
  [CHORE_DUE_STATES.ALMOST_DUE]: 'warn',
  [CHORE_DUE_STATES.OVERDUE]: 'error',
};
const severity = computed(() => SEVERITIES[props.chore.dueState]);
const message = computed(() => getDueDaysMessage(props.chore));
</script>
