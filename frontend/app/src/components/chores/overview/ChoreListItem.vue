<template>
  <router-link
    :to="{
      name: routes.chores.children.detail.name,
      params: {
        id: chore.id,
      },
    }"
  >
    <li
      class="chore-list-item"
      :class="stateClass"
    >
      <div class="left">
        <div class="name">{{ chore.name }}</div>
        <ChoreTags :tags="chore.tags" />
      </div>
      <div class="right">
        <ChoreListItemNextDue :chore />
      </div>
    </li>
  </router-link>
</template>

<script setup>
import { computed } from 'vue';
import { routes } from '@/router/routes.js';
import ChoreTags from '@/components/chores/common/ChoreTags.vue';
import ChoreListItemNextDue from '@/components/chores/overview/ChoreListItemNextDue.vue';
import { CHORE_DUE_STATES } from '@/utilities/chores.js';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});

const STATE_CLASSES = {
  [CHORE_DUE_STATES.OK]: 'ok',
  [CHORE_DUE_STATES.ALMOST_DUE]: 'almost-due',
  [CHORE_DUE_STATES.OVERDUE]: 'overdue',
};
const stateClass = computed(() => STATE_CLASSES[props.chore.dueState]);
</script>

<style scoped lang="scss">
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';
@import '@/styles/_shadows.scss';

.chore-list-item {
  @include white-content-box;
  @include flex-row-justify-between-wrapping;
  gap: var(--default-padding);

  transition: all 150ms;

  &:hover {
    @include shadow-2;
  }

  .left {
    @include flex-column;
    gap: 0.5rem;

    .name {
      font-size: 120%;
      font-weight: 700;
    }
  }

  .right {
    @include flex-row-actions;
  }

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
