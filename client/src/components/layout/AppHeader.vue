<template>
  <header>
    <div class="brand">
      <div class="icon">icon</div>
      Chores
    </div>
    <div class="toggle">
      <PrimeButton
        icon="pi pi-bars"
        severity="secondary"
        @click="showMobileMenu = !showMobileMenu"
      />
    </div>
    <div
      class="content"
      :class="{ visible: showMobileMenu }"
    >
      <ul class="links">
        <li
          v-for="route in navigationRoutes"
          :key="route.name"
        >
          <router-link
            :to="{ name: route.name }"
            class="action-item"
            @click="showMobileMenu = false"
          >
            <i
              class="pi"
              :class="route.icon"
            />
            <span>{{ route.label }}</span>
          </router-link>
        </li>
      </ul>
      <ul class="actions">
        <li
          v-for="action in actions"
          :key="action.label"
          class="action-item"
        >
          <i
            class="pi"
            :class="action.icon"
          />
          <span>{{ action.label }}</span>
        </li>
      </ul>
    </div>
  </header>
</template>

<script setup>
import { ref } from 'vue';
import { routes } from '@/router/routes';

const showMobileMenu = ref(false);
const navigationRoutes = [
  {
    name: routes.chores.name,
    label: 'Chores',
    icon: 'pi-home',
  },
  {
    name: routes.tags.name,
    label: 'Tags',
    icon: 'pi-tags',
  },
];
const actions = [
  {
    label: 'Inbox',
    icon: 'pi-inbox',
  },
  {
    label: 'Notifications',
    icon: 'pi-bell',
  },
];
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';
@import '@/styles/_custom-vars.scss';
@import '@/styles/_shadows.scss';

header {
  min-height: 80px;
  background-color: var(--surface-overlay);
  margin-bottom: var(--default-padding);

  @include flex-row-justify-between;
  align-items: center;

  @include padding-x(var(--default-padding));

  position: sticky;
  top: 0;
  z-index: 1;

  @include shadow-2;
}

.brand {
  height: 40px;
  font-size: 150%;
  font-weight: 700;
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;

  .icon {
    height: 100%;
    display: inline-flex;
    align-items: center;
    font-size: 50%;
    margin-right: 0.75rem;
    @include padding-x(0.5rem);
    border: 1px solid var(--p-surface-700);
  }

  @include media-min-width($md) {
    margin-right: 3rem; // TODO less?
  }
}

.toggle {
  display: block;
  @include media-min-width($md) {
    display: none;
    //color: var(--p-text-700);
  }
}

.content {
  flex-grow: 1;
  @include flex-row-justify-between;

  display: none;
  @include media-min-width($md) {
    display: flex;
  }

  &.visible {
    display: flex;
  }

  flex-direction: column;
  @include media-min-width($md) {
    flex-direction: row;
  }

  position: absolute;
  @include media-min-width($md) {
    position: static;
  }

  width: 100%;
  left: 0;
  top: 100%;
  z-index: 1;
  background-color: var(--surface-overlay);
  // TODO shadow (only bottom border)
}

.links,
.actions {
  list-style: none;
  padding: 0;
  margin: 0;

  @include flex-column;
  @include media-min-width($md) {
    flex-direction: row;
    gap: 0.5rem;
  }

  //user-select: none;

  .action-item {
    @include flex;
    align-items: center;

    @include padding-x(2rem);
    @include media-min-width($md) {
      @include padding-x(1rem);
    }
    @include padding-y(1rem);

    @include media-min-width($md) {
      border-radius: var(--border-radius);
    }
    font-weight: 500; // TODO add font
    cursor: pointer; // TODO replace w/ link/button

    // TODO transition
    &:hover {
      background-color: var(--surface-hover);
    }

    .pi {
      margin-right: 0.5rem;
    }
  }
}

.links {
  // router-link is replaced w/ <a> tag
  // eslint-disable-next-line vue-scoped-css/no-unused-selector
  a {
    &.router-link-active {
      background-color: var(--surface-ground);
      color: var(--p-text-900);
    }
  }
}

.actions {
  @include media-max-width($md) {
    border-top: solid 1px var(--surface-border);
  }

  .action-item {
    @include media-min-width($md) {
      .pi {
        font-size: 1.5rem;
        margin-right: 0;
      }

      span {
        display: none;
      }
    }
  }
}
</style>
