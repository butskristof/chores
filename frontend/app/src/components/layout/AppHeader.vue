<template>
  <header>
    <router-link
      :to="{ name: routes.home.name }"
      class="brand"
    >
      <div class="logo-wrapper">
        <img
          :src="
            darkMode.isDark.value === true ? '/images/chores-light.svg' : '/images/chores-dark.svg'
          "
          alt="Chores"
        />
      </div>
      <div>Chores</div>
    </router-link>
    <div
      v-if="isAuthenticated"
      class="toggle"
    >
      <PrimeButton
        icon="pi pi-bars"
        severity="secondary"
        @click="showMobileMenu = !showMobileMenu"
      />
    </div>
    <div
      v-if="isAuthenticated"
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
import { useAppDarkMode } from '@/composables/app.js';
import PrimeButton from 'primevue/button';
import { useBffUser } from '@/composables/queries/auth.js';

const darkMode = useAppDarkMode();
const { isAuthenticated } = useBffUser();

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
  background-color: var(--overlay-background);
  margin-bottom: var(--default-padding);

  @include flex-row-justify-between;
  align-items: center;

  padding-inline: var(--default-padding);

  position: sticky;
  top: 0;
  z-index: 1;

  @include shadow-2;
}

.brand {
  font-size: 150%;
  font-weight: 700;
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 0.5rem;

  .logo-wrapper {
    width: 36px;

    img {
      display: block;
      width: 100%;
      height: 100%;
    }
  }

  @include media-min-width($md) {
    margin-right: 2rem;
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
  background-color: var(--overlay-background);
}

.links,
.actions {
  @include unstyled-ul;
  @include flex-column;
  @include media-min-width($md) {
    flex-direction: row;
    gap: 0.5rem;
  }

  //user-select: none;

  .action-item {
    @include flex;
    align-items: center;

    padding-inline: 2rem;
    @include media-min-width($md) {
      padding-inline: 1rem;
    }
    padding-block: 1rem;

    @include media-min-width($md) {
      border-radius: var(--border-radius);
    }
    font-weight: 500; // TODO add font
    cursor: pointer; // TODO replace w/ link/button

    // TODO transition
    &:hover {
      background-color: var(--hover-background);
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
      background-color: var(--ground-background);
      // TODO
      color: var(--p-text-900);
    }
  }
}

.actions {
  @include media-max-width($md) {
    border-top: 1px solid var(--border-color);
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
