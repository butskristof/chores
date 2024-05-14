<template>
  <div class="items">
    <router-link :to="{ name: routes.chores.children.detail.name, params: { id: chore.id } }">
      <li
        class="chore-list-item"
        :class="STATE_INFO[state].class"
      >
        <div class="details">
          <div class="name-tags">
            <div class="name">{{ chore.name }}</div>
            <div
              v-if="chore.tags.length > 0"
              class="tags"
            >
              <PrimeTag
                v-for="tag in chore.tags"
                :key="tag.id"
                :value="tag.name"
                severity="secondary"
              />
            </div>
          </div>
        </div>
        <div class="actions">
          <div>due X days</div>
          <PrimeAvatar
            :icon="STATE_INFO[state].icon"
            size="large"
            shape="circle"
            class="state-icon"
            :class="STATE_INFO[state].class"
          />
        </div>
      </li>
    </router-link>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import PrimeTag from 'primevue/tag';
import PrimeAvatar from 'primevue/avatar';
import { routes } from '@/router/routes.js';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
  // TODO remove
  index: {
    type: Number,
    required: true,
  },
});

const STATES = {
  OK: 'OK',
  ALMOST_DUE: 'ALMOST_DUE',
  OVERDUE: 'OVERDUE',
};
const STATE_INFO = {
  [STATES.OK]: {
    class: 'ok',
    severity: 'success',
    icon: 'pi pi-check',
  },
  [STATES.ALMOST_DUE]: {
    class: 'almost-due',
    severity: 'warn',
    icon: 'pi pi-exclamation-triangle',
  },
  [STATES.OVERDUE]: {
    class: 'overdue',
    severity: 'danger',
    icon: 'pi pi-times',
  },
};

const state = computed(() => Object.values(STATES)[props.index % Object.keys(STATES).length]);
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
    background-color: var(--p-green-50);
    border-color: var(--p-green-500);
    color: var(--p-green-700);
  }

  &.almost-due {
    background-color: var(--p-yellow-50);
    border-color: var(--p-yellow-500);
    color: var(--p-yellow-700);
  }

  &.overdue {
    background-color: var(--p-red-50);
    border-color: var(--p-red-500);
    color: var(--p-red-700);
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
    .tags {
      @include flex-row;
      flex-wrap: wrap;
      gap: 0.5rem;
    }
  }
}

.state-icon {
  flex-shrink: 0;
  border: 2px solid;

  &.ok {
    background-color: var(--p-green-50);
    border-color: var(--p-green-500);
  }

  &.almost-due {
    background-color: var(--p-yellow-50);
    border-color: var(--p-yellow-500);
  }

  &.overdue {
    background-color: var(--p-red-50);
    border-color: var(--p-red-500);
  }
}

.actions {
  @include flex-row;
  align-items: center;
  gap: 1rem;
}
</style>
