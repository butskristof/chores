<template>
  <div class="items">
    <router-link :to="{ name: routes.chores.children.detail.name, params: { id: chore.id } }">
      <li
        class="chore-list-item"
        :class="stateClass"
      >
        <div class="details">
          <div class="name-tags">
            <div class="name">{{ chore.name }}</div>
            <ChoreTags :tags="chore.tags" />
          </div>
        </div>
        <div class="actions">
          <ChoreListItemNextDue :chore />
        </div>
      </li>
    </router-link>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import { routes } from '@/router/routes.js';
import ChoreTags from '@/components/chores/common/ChoreTags.vue';
import ChoreListItemNextDue from '@/components/chores/overview/ChoreListItemNextDue.vue';
import { CHORE_DUE_STATES, getChoreDueState } from '@/utilities/chores.js';

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

const state = computed(() => getChoreDueState(props.chore));
const stateClass = computed(() => STATE_CLASSES[state.value]);
</script>

<style scoped lang="scss">
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';
@import '@/styles/_shadows.scss';

.chore-list-item {
  @include flex-row-justify-between-wrapping;
  gap: var(--default-padding);
  border-radius: var(--border-radius);

  &:hover {
    @include shadow-2;
  }

  padding: 1rem;
  border: 1px solid;

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

.details {
  @include flex-row;
  align-items: center;
  gap: 1rem;

  .name-tags {
    @include flex-column;
    gap: 0.5rem;
    .name {
      font-size: 120%;
      font-weight: 700;
    }
  }
}

.actions {
  @include flex-row;
  align-items: center;
  gap: 1rem;
}
</style>
