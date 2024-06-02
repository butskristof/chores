<template>
  <div
    v-dompurify-html="content"
    class="ql-editor"
  />
</template>

<script setup>
defineProps({
  content: {
    type: String,
    default: () => null,
  },
});
</script>
