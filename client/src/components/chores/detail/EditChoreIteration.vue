<template>
  <Dialog
    :visible="true"
    modal
    :draggable="false"
    :header="isEdit ? 'Edit chore' : 'Create new chore'"
    :style="{ width: '50rem' }"
    :breakpoints="{ '1199px': '75vw', '575px': '90vw' }"
    @update:visible="updateVisible"
  >
    <form @submit="save">
      <div class="field">
        <label for="date">Date</label>
        <Calendar
          id="date"
          v-model="date.value.value"
          autofocus
          date-format="dd/mm/yy"
          :disabled="isFormDisabled"
          :class="{ 'p-invalid': date.errorMessage.value }"
        />
        <small
          v-if="date.errorMessage"
          class="p-error"
          >{{ date.errorMessage }}</small
        >
      </div>

      <div class="field">
        <label for="notes">Notes</label>
        <InputText
          id="notes"
          v-model.trim="notes.value.value"
          autofocus
          type="text"
          :disabled="isFormDisabled"
          :class="{ 'p-invalid': notes.errorMessage.value }"
        />
        <small
          v-if="notes.errorMessage"
          class="p-error"
          >{{ notes.errorMessage }}</small
        >
      </div>

      <div class="footer">
        <div class="result">
          <InlineMessage
            v-if="mutation.isSuccess.value === true"
            severity="success"
            >Iteration saved</InlineMessage
          >
        </div>
        <div class="actions">
          <Button
            type="submit"
            label="Save"
            icon="pi pi-save"
            :disabled="isFormDisabled"
            :loading="mutation.isPending.value"
          />
        </div>
      </div>
    </form>
  </Dialog>
</template>

<script setup>
import { useQueryClient } from '@tanstack/vue-query';
import { useToast } from 'vue-toastification';
import { computed } from 'vue';
import { useForm, useField } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import { useChoresApiUpsertChoreIteration } from '@/composables/queries/chores-api';
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import InlineMessage from 'primevue/inlinemessage';
import Calendar from 'primevue/calendar';

const props = defineProps({
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
const isEdit = computed(() => props.iteration != null);

const queryClient = useQueryClient();
const toast = useToast();

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

const isFormDisabled = computed(
  () => mutation.isPending.value === true || mutation.isSuccess.value === true,
);

const date = useField('date');
const notes = useField('notes');

//#endregion

//#region create/update

const mutation = useChoresApiUpsertChoreIteration(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    const payload = {
      choreId: props.choreId,
      date: new Date(values.date).toISOString(),
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

const updateVisible = (value) => {
  if (value === false) tryClose();
};
const tryClose = (force = false) => {
  let close = true;
  if (!force && meta.value.dirty && mutation.isSuccess.value !== true)
    close = confirm('There may be unsaved changes, are you sure you want to stop editing?');
  if (close) emit('close');
};
</script>

<style scoped lang="scss">
form {
  .field {
    margin-bottom: 1rem;

    label {
      display: block;
      margin-bottom: 0.5rem;
    }

    :deep(input) {
      width: 100%;
    }
  }

  .footer {
    display: flex;
    flex-direction: row;
    justify-content: space-between;

    .actions {
      display: flex;
      flex-direction: row;
      justify-content: flex-end;
    }
  }
}
</style>
