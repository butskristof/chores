<template>
  <div
    class="next-due"
    :class="severityClass"
  >
    <div>{{ message }}</div>
    <PrimeAvatar
      :icon="icon"
      size="large"
      shape="circle"
      class="state-icon"
      :class="severityClass"
    />
  </div>
</template>

<script setup>
import PrimeAvatar from 'primevue/avatar';
import { computed } from 'vue';
import { CHORE_DUE_STATES, getChoreDueState, getDueDays } from '@/utilities/chores.js';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
const dueDays = computed(() => getDueDays(props.chore));
const state = computed(() => getChoreDueState(props.chore));
const CLASSES = {
  [CHORE_DUE_STATES.OK]: 'ok',
  [CHORE_DUE_STATES.ALMOST_DUE]: 'almost-due',
  [CHORE_DUE_STATES.OVERDUE]: 'overdue',
};
const ICONS = {
  [CHORE_DUE_STATES.OK]: 'pi pi-check',
  [CHORE_DUE_STATES.ALMOST_DUE]: 'pi pi-exclamation-triangle',
  [CHORE_DUE_STATES.OVERDUE]: 'pi pi-times',
};
const severityClass = computed(() => CLASSES[state.value]);
const icon = computed(() => ICONS[state.value] ?? ICONS[CHORE_DUE_STATES.ALMOST_DUE]);
const message = computed(() => {
  if (dueDays.value == null) return 'due now';
  if (dueDays.value < 0) return `due ${Math.abs(dueDays.value)} days ago`;
  else if (dueDays.value === 1) return `due tomorrow`;
  else if (dueDays.value === 0) return `due today`;
  return `due in ${dueDays.value} days`;
});
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.next-due {
  @include flex-row;
  align-items: center;
  gap: 1rem;

  margin-left: 1rem;
}

.state-icon {
  flex-shrink: 0;
  border: 2px solid;

  &.ok {
    @include ok;
  }

  &.almost-due {
    @include almost-due;
  }

  &.overdue {
    @include overdue;
  }
}
</style>
