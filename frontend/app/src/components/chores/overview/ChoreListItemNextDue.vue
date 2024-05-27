<template>
  <div class="next-due">
    <div>{{ message }}</div>
    <PrimeAvatar
      :icon="icon"
      size="large"
      shape="circle"
      class="state-icon"
    />
  </div>
</template>

<script setup>
import PrimeAvatar from 'primevue/avatar';
import { computed } from 'vue';
import { getDueDaysMessage, getDueStateIcon } from '@/utilities/chores.js';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

const icon = computed(() => getDueStateIcon(props.chore));
const message = computed(() => getDueDaysMessage(props.chore));
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
  background-color: transparent;
}
</style>
