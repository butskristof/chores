<template>
  <AppDialog
    :open="open"
    @close="tryClose"
  >
    <div
      v-if="mutation.isSuccess.value === true"
      class="success"
    >
      <p>Iteration was {{ isEdit ? 'updated' : 'created' }} successfully.</p>
      <div class="actions">
        <button
          type="button"
          @click="tryClose(true)"
        >
          Close
        </button>
      </div>
    </div>
    <template v-else>
      <DialogTitle>
        {{ isEdit ? 'Edit iteration' : 'Create new iteration' }}
      </DialogTitle>
      <form @submit="save">
        <TextInput
          label="Date"
          name="date"
          :disabled="formDisabled"
        />

        <TextInput
          label="Notes"
          name="notes"
          :disabled="formDisabled"
        />

        <div class="actions">
          <button
            type="submit"
            :disabled="formDisabled"
          >
            <span v-if="mutation.isPending.value === true">Saving...</span>
            <span v-else-if="isEdit">Save changes</span>
            <span v-else>Create</span>
          </button>
        </div>
      </form>
    </template>
  </AppDialog>
</template>

<script setup>
import AppDialog from '@/components/common/dialogs/AppDialog.vue';
import { DialogTitle } from '@headlessui/vue';
import { useQueryClient } from '@tanstack/vue-query';
import { useToast } from 'vue-toastification';
import { computed } from 'vue';
import { useForm } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import { useChoresApiUpsertChoreIteration } from '@/composables/queries/chores-api';
import TextInput from '@/components/common/form/inputs/TextInput.vue';

const props = defineProps({
  open: {
    type: Boolean,
    default: false,
  },
  choreId: {
    type: String,
    required: true,
  },
  iteration: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);

const queryClient = useQueryClient();
const toast = useToast();

const isEdit = computed(() => props.iteration != null);

//#region form

const { handleSubmit, meta } = useForm({
  validationSchema: toTypedSchema(
    yup.object({
      date: yup.string().required().label('Date'),
      notes: yup.string().label('Notes'),
    }),
  ),
  initialValues: {
    date: props.iteration?.date,
    notes: props.iteration?.notes,
  },
});

const formDisabled = computed(
  () => mutation.isPending.value === true || mutation.isSuccess.value === true,
);

//#endregion

//#region create/update

const mutation = useChoresApiUpsertChoreIteration(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    const payload = {
      choreId: props.choreId,
      date: values.date,
      notes: values.notes,
    };
    if (isEdit.value === true) payload.iterationId = props.iteration.id;
    await mutation.mutateAsync(payload);
    toast.success(isEdit.value === true ? 'Iteration updated' : 'Iteration created');
    tryClose(true);
  } catch (e) {
    console.error(e);
  }
});

//#endregion

const tryClose = (force = false) => {
  let close = true;
  if (!force && meta.value.dirty)
    close = confirm('There may be unsaved changes, are you sure you want to stop editing?');
  if (close) emit('close');
};
</script>

<style scoped lang="scss">
.actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
}
</style>
