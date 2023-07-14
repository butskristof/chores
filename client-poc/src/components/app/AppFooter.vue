<template>
  <footer>
    <div class="links">
      <div class="github">
        <Github class="icon" />
      </div>
    </div>
  </footer>
</template>

<script setup>
import { Github } from 'lucide-vue-next';
</script>

<style scoped lang="scss">
@import '@/styles/_variables.scss';
@import '@/styles/_utilities.scss';

footer {
  margin-top: calc(2 * $box-padding);
  margin-bottom: $box-padding;
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
}

.icon {
  @include icon-font-size;
}
</style>
