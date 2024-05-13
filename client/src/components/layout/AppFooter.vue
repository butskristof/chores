<template>
  <footer>
    <div class="actions">
      <PrimeButton
        aria-label="Toggle dark mode"
        :icon="darkMode.isDark === true ? 'pi pi-sun' : 'pi pi-moon'"
        text
        rounded
        severity="secondary"
        @click="darkMode.toggle()"
      />
      <a href="https://github.com/butskristof/chores">
        <PrimeButton
          icon="pi pi-github"
          text
          rounded
          severity="secondary"
          aria-label="GitHub"
        />
      </a>
      <a href="#">
        <PrimeButton
          icon="pi pi-cog"
          text
          rounded
          severity="secondary"
          aria-label="Settings"
        />
      </a>
    </div>
  </footer>
</template>

<script setup>
import { useAppDarkMode } from '@/composables/utilities.js';

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
