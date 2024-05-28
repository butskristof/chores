<template>
  <OhVueIcon :name />
</template>

<script setup>
import { OhVueIcon } from 'oh-vue-icons';
defineProps({
  name: {
    type: String,
    required: true,
  },
});
</script>

<style scoped lang="scss"></style>
