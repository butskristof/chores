<template>
  <Dialog
    :visible="true"
    modal
    :draggable="false"
    header="Edit chore tags"
    :style="{ width: '50rem' }"
    :breakpoints="{ '1199px': '75vw', '575px': '90vw' }"
    @update:visible="updateVisible"
  >
    <div class="field">
      <Multiselect
        v-model="selectedTags"
        :options="tags"
        label="name"
        track-by="id"
        :multiple="true"
      />
    </div>

    <div class="actions">
      <Button
        type="button"
        label="Save"
        icon="pi pi-save"
        @click="save"
      />
    </div>
  </Dialog>
</template>

<script setup>
import { useChoresApiTags, useChoresApiUpdateChoreTags } from '@/composables/queries/chores-api.js';
import { computed, ref } from 'vue';
import { useQueryClient } from '@tanstack/vue-query';
import { useToast } from 'vue-toastification';
import Multiselect from 'vue-multiselect';

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

const updateVisible = (value) => {
  if (value === false) emit('close');
};
</script>

<style scoped lang="scss">
.actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
}

.field {
  margin-bottom: 1rem;
}
</style>
