<template>
  <footer>
    <div class="actions">
      <a href="https://github.com/butskristof/chores">
        <i class="pi pi-github" />
      </a>
    </div>
  </footer>
</template>

<script setup></script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

footer {
  @include flex-row;
  justify-content: flex-end;
  padding: var(--default-padding);

  .actions {
    @include flex-row;
    gap: 0.5rem;
  }
}
</style>
