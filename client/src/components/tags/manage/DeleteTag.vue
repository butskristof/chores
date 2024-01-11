<template>
  <Toast />
  <ConfirmDialog />
</template>

<script setup>
import Toast from 'primevue/toast';
import ConfirmDialog from 'primevue/confirmdialog';
import { useQueryClient } from '@tanstack/vue-query';
import { useChoreApiDeleteTag } from '@/composables/queries/chores-api.js';
import { useConfirm } from 'primevue/useconfirm';
import { onMounted } from 'vue';
import { useToast } from 'vue-toastification';

const props = defineProps({
  tag: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);

const confirm = useConfirm();
const queryClient = useQueryClient();
const toast = useToast();

const mutation = useChoreApiDeleteTag(queryClient);

const deleteTag = async () => {
  try {
    await mutation.mutateAsync(props.tag.id);
    toast.success('Tag deleted');
    emit('close');
  } catch (e) {
    console.error(e);
  }
};

onMounted(() => {
  confirm.require({
    header: 'Delete tag?',
    icon: 'pi pi-danger',
    rejectClass: 'p-button-text p-button-text',
    acceptClass: 'p-button-danger p-button-text',
    accept: deleteTag,
    reject: () => emit('close'),
  });
});
</script>

<style scoped lang="scss"></style>
