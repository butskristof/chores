<template>
  <footer>
    <div class="actions">
      <Tippy
        content="Toggle colour scheme"
        :hide-on-click="false"
      >
        <PrimeButton
          :icon="darkMode.isDark.value === true ? 'pi pi-sun' : 'pi pi-moon'"
          text
          rounded
          severity="secondary"
          aria-label="Toggle colour scheme"
          @click="darkMode.toggle()"
        />
      </Tippy>
      <Tippy content="GitHub">
        <a href="https://github.com/butskristof/chores">
          <PrimeButton
            icon="pi pi-github"
            text
            rounded
            severity="secondary"
            aria-label="GitHub"
          />
        </a>
      </Tippy>
      <Tippy content="Settings">
        <a href="#">
          <PrimeButton
            icon="pi pi-cog"
            text
            rounded
            severity="secondary"
            aria-label="Settings"
          />
        </a>
      </Tippy>
    </div>
  </footer>
</template>

<script setup>
import PrimeButton from 'primevue/button';
import { useAppDarkMode } from '@/composables/app.js';
import { Tippy } from 'vue-tippy';

const darkMode = useAppDarkMode();
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

footer {
  @include flex-row;
  justify-content: flex-end;
  padding: var(--default-padding);

  .actions {
    @include flex-row;
    align-items: center;
    gap: 0.5rem;
  }
}
</style>
