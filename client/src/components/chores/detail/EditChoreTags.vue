<template>
  <EditDialog
    :maximizable="false"
    header="Edit chore tags"
    @close="$emit('close')"
  >
    <div class="edit-chore-tags">
      <div class="field">
        <PrimeMultiSelect
          v-model="selectedTags"
          :options="tags"
          option-label="name"
          option-value="id"
          filter
          :max-selected-labels="3"
        />
      </div>
      <div class="footer">
        <div class="result">
          <PrimeInlineMessage
            v-if="mutation.isSuccess.value"
            severity="success"
            >Tags saved</PrimeInlineMessage
          >
          <ApiError
            v-if="mutation.isError.value"
            :error="mutation.error.value"
          >
            <template #message>
              <p>
                Saving the tags failed, please refer to the error information below for more
                details.
              </p>
            </template>
          </ApiError>
        </div>
        <div class="actions">
          <PrimeButton
            label="Save"
            icon="pi pi-save"
            :loading="mutation.isPending.value"
            @click="save"
          />
        </div>
      </div>
    </div>
  </EditDialog>
</template>

<script setup>
import { useQueryClient } from '@tanstack/vue-query';
import { useChoresApiTags, useChoresApiUpdateChoreTags } from '@/composables/queries/chores-api.js';
import { useToast } from 'vue-toastification';
import { ref } from 'vue';
import PrimeButton from 'primevue/button';
import PrimeMultiSelect from 'primevue/multiselect';
import PrimeInlineMessage from 'primevue/inlinemessage';
import ApiError from '@/components/common/ApiError.vue';
import EditDialog from '@/components/common/EditDialog.vue';

const props = defineProps({
  chore: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(['close']);

const { tags } = useChoresApiTags();
const selectedTags = ref(props.chore.tags.map((t) => t.id));

//#region update

const toast = useToast();
const queryClient = useQueryClient();
const mutation = useChoresApiUpdateChoreTags(queryClient);
const save = async () => {
  try {
    const payload = {
      choreId: props.chore.id,
      tagIds: [...selectedTags.value],
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
@import '@/styles/_utilities.scss';

.edit-chore-tags {
  @include form-styling;
}
</style>
