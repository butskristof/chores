<template>
  <div class="color-scheme-picker">
    <select v-model="mode">
      <option
        v-for="option in options"
        :key="option"
        :value="option"
      >
        {{ option }}
      </option>
    </select>
  </div>
</template>

<script setup>
import { useColorMode } from '@vueuse/core';

const options = ['auto', 'light', 'dark', 'dim'];
const mode = useColorMode({
  attribute: 'color-scheme',
  modes: {
    dim: 'dim',
  },
});
</script>

<style scoped lang="scss"></style>
