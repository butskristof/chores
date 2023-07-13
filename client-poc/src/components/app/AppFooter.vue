<template>
  <footer>
    <div class="copyright">&copy;</div>
    <div class="links">
      <div class="github">
        <Github />
      </div>
    </div>
  </footer>
</template>

<script setup>
import { Github } from 'lucide-vue-next';
</script>

<style scoped lang="scss">
@import '@/styles/_variables.scss';

footer {
  margin-bottom: $box-padding;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}
</style>
