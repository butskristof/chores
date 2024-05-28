<template>
  <OhVueIcon
    v-if="valid"
    :name
  />
</template>

<script setup>
import { OhVueIcon } from 'oh-vue-icons';
import { computed } from 'vue';
import { ICON_NAMES } from '@/utilities/icons.js';
const props = defineProps({
  name: {
    type: String,
    required: true,
  },
});
const valid = computed(() => ICON_NAMES.includes(props.name));
</script>

<style scoped lang="scss"></style>
