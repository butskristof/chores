<template>
  <footer>
    <div class="copyright">&copy;</div>
    <div class="links">github</div>
  </footer>
</template>

<script setup></script>

<style scoped lang="scss">
@import '@/styles/_variables.scss';

footer {
  margin-bottom: $box-padding;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}
</style>
