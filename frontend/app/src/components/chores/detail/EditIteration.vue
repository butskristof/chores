<template>
  <EditDialog
    :header="isEdit ? 'Edit iteration' : 'Create new iteration'"
    @close="tryClose"
  >
    <form @submit="save">
      <div class="field">
        <label for="date">Date</label>
        <PrimeDatePicker
          id="date"
          v-model="date.value.value"
          :invalid="date.errors.value.length > 0"
          :disabled="isFormDisabled"
          date-format="dd/mm/yy"
          show-icon
          show-button-bar
          :max-date="maxDate"
        />
        <small
          v-if="date.errors.value.length > 0"
          class="p-error"
        >
          <span v-if="date.errors.value.length === 1">{{ date.errorMessage.value }}</span>
          <template v-else>
            <span
              v-for="error in date.errors.value"
              :key="error"
              >{{ error }} <br
            /></span>
          </template>
        </small>
      </div>

      <div class="field">
        <label for="notes">Notes</label>
        <PrimeEditor
          id="notes"
          v-model="notes.value.value"
          :readonly="isFormDisabled"
          editor-style="height: 10rem;"
          @load="initializeNotesEditor"
        />
        <small
          v-if="notes.errors.value.length > 0"
          class="p-error"
        >
          <span v-if="notes.errors.value.length === 1">{{ notes.errorMessage.value }}</span>
          <template v-else>
            <span
              v-for="error in notes.errors.value"
              :key="error"
              >{{ error }} <br
            /></span>
          </template>
        </small>
      </div>

      <div class="footer">
        <div class="result">
          <PrimeInlineMessage
            v-if="mutation.isSuccess.value"
            severity="success"
            >Iteration saved</PrimeInlineMessage
          >
          <ApiError
            v-if="mutation.isError.value"
            :error="mutation.error.value"
          >
            <template #message>
              <p>
                Saving the iteration failed, please refer to the error information below for more
                details.
              </p>
            </template>
          </ApiError>
        </div>
        <div class="actions">
          <PrimeButton
            type="submit"
            label="Save"
            icon="pi pi-save"
            :disabled="isFormDisabled"
            :loading="mutation.isPending.value"
          />
        </div>
      </div>
    </form>
  </EditDialog>
</template>

<script setup>
import { computed } from 'vue';
import { useQueryClient } from '@tanstack/vue-query';
import { useChoresApiUpsertChoreIteration } from '@/composables/queries/chores-api.js';
import { useField, useForm } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import PrimeInlineMessage from 'primevue/inlinemessage';
import PrimeButton from 'primevue/button';
import ApiError from '@/components/common/ApiError.vue';
import PrimeDatePicker from 'primevue/datepicker';
import { formatDateAsJson } from '@/utilities/datetime.js';
import EditDialog from '@/components/common/EditDialog.vue';
import { generateQuillInitializer } from '@/utilities/editor.js';
import PrimeEditor from 'primevue/editor';

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

//#region form
const { handleSubmit, meta } = useForm({
  validationSchema: toTypedSchema(
    yup.object({
      date: yup.date().required().label('Date'),
      notes: yup.string().trim().label('Notes'),
    }),
  ),
  initialValues: {
    date:
      props.iteration?.date ??
      (() => {
        const now = new Date();
        return new Date(now.getFullYear(), now.getMonth(), now.getDate());
      })(),
    notes: props.iteration?.notes,
  },
});

const isFormDisabled = computed(
  () => mutation.isPending.value === true || mutation.isSuccess.value === true,
);

const maxDate = new Date();
const date = useField('date');
const notes = useField('notes');
const initializeNotesEditor = generateQuillInitializer(notes.value);
//#endregion

//#region create/update

const queryClient = useQueryClient();
const mutation = useChoresApiUpsertChoreIteration(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    const payload = {
      choreId: props.choreId,
      date: formatDateAsJson(values.date),
      notes: values.notes,
    };
    // if (isEdit.value === true) payload.id = props.iteration.id;
    await mutation.mutateAsync(payload);
  } catch (e) {
    console.error(e);
  }
});

//#endregion

const tryClose = (force = false) => {
  let close = true;
  if (!force && meta.value.dirty && mutation.isSuccess.value !== true)
    // TODO primevue confirm?
    close = confirm('There may be unsaved changes, are you sure you want to stop editing?');
  if (close) emit('close');
};
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

form {
  @include form-styling;
}
</style>
