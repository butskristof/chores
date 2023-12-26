<template>
  <AppDialog :open="true">
    <DialogTitle>Edit chore tags</DialogTitle>

    <Multiselect
      v-model="selectedTags"
      :options="tags"
      label="name"
      track-by="id"
      :multiple="true"
    />

    <div class="actions">
      <button
        type="button"
        @click="save"
      >
        save
      </button>
    </div>
  </AppDialog>
</template>

<script setup>
import AppDialog from '@/components/common/dialogs/AppDialog.vue';
import { DialogTitle } from '@headlessui/vue';
import { useChoresApiTags, useChoresApiUpdateChoreTags } from '@/composables/queries/chores-api.js';
import { computed, ref } from 'vue';
import Multiselect from 'vue-multiselect';
import { useQueryClient } from '@tanstack/vue-query';
import { useToast } from 'vue-toastification';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(['close']);

const tagsQuery = useChoresApiTags();
const tags = computed(() => tagsQuery.data.value?.tags ?? []);
const selectedTags = ref(props.chore.tags);

//#region update
const queryClient = useQueryClient();
const toast = useToast();
const mutation = useChoresApiUpdateChoreTags(queryClient);
const save = async () => {
  try {
    const payload = {
      choreId: props.chore.id,
      tagIds: selectedTags.value.map((t) => t.id),
    };
    await mutation.mutateAsync(payload);
    toast.success('Chore tags updated');
    emit('close');
  } catch (e) {
    console.error(e);
  }
};
//#endregion
</script>

<style scoped lang="scss">
.actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
}
</style>
