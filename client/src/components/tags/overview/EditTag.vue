<template>
  <AppDialog
    :open="open"
    @close="emit('close')"
  >
    <template #title> Create new tag </template>
    <template #default>
      <form>
        <div class="actions">
          <button type="submit">Create</button>
        </div>
      </form>
    </template>
  </AppDialog>
</template>

<script setup>
import AppDialog from '@/components/common/dialogs/AppDialog.vue';

defineProps({
  open: {
    type: Boolean,
    default: false,
  },
  // tag: {
  //   type: Object,
  //   required: true,
  // },
});
const emit = defineEmits(['close']);

// const queryClient = useQueryClient();
// const toast = useToast();
</script>

<style scoped lang="scss">
.actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
}
</style>
