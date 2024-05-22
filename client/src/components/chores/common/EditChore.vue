<template>
  <PrimeDialog
    visible
    modal
    maximizable
    :draggable="false"
    :header="isEdit ? 'Edit chore' : 'Create new chore'"
    :style="{
      width: '50rem',
    }"
    @update:visible="updateVisible"
  >
    <form @submit="save">
      <div class="field">
        <label for="name">Name</label>
        <PrimeInputText
          id="name"
          v-model.trim="name.value.value"
          type="text"
          :disabled="isFormDisabled"
          :invalid="name.errors.value.length > 0"
        />
        <small
          v-if="name.errors.value.length > 0"
          class="p-error"
        >
          <span v-if="name.errors.value.length === 1">{{ name.errorMessage.value }}</span>
          <template v-else>
            <span
              v-for="error in name.errors.value"
              :key="error"
              >{{ error }} <br
            /></span>
          </template>
        </small>
      </div>

      <div class="field">
        <label for="interval">Interval</label>
        <PrimeInputNumber
          id="interval"
          v-model.number="interval.value.value"
          :disabled="isFormDisabled"
          :invalid="interval.errors.value.length > 0"
          :min="1"
        />
        <small
          v-if="interval.errors.value.length > 0"
          class="p-error"
        >
          <span v-if="interval.errors.value.length === 1">{{ interval.errorMessage.value }}</span>
          <template v-else>
            <span
              v-for="error in interval.errors.value"
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
            >Chore saved</PrimeInlineMessage
          >
          <ApiError
            v-if="mutation.isError.value"
            :error="mutation.error.value"
          >
            <template #message>
              <p>
                Saving the chore failed, please refer to the error information below for more
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
  </PrimeDialog>
</template>

<script setup>
import { computed } from 'vue';
import { useField, useForm } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import { useQueryClient } from '@tanstack/vue-query';
import { useChoresApiUpsertChore } from '@/composables/queries/chores-api.js';
import PrimeDialog from 'primevue/dialog';
import PrimeInlineMessage from 'primevue/inlinemessage';
import PrimeButton from 'primevue/button';
import ApiError from '@/components/common/ApiError.vue';
import PrimeInputText from 'primevue/inputtext';
import PrimeInputNumber from 'primevue/inputnumber';

// TODO enable buttons on interval field when fixed
// show-buttons
// button-layout="horizontal"
// https://github.com/primefaces/primevue/issues/5754

const props = defineProps({
  chore: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);
const isEdit = computed(() => props.chore != null);

//#region form
const { handleSubmit, meta } = useForm({
  validationSchema: toTypedSchema(
    yup.object({
      name: yup.string().required().label('Name'),
      interval: yup.number().required().positive().integer().label('Interval'),
    }),
  ),
  initialValues: {
    name: props.chore?.name,
    interval: props.chore?.interval,
  },
});

const isFormDisabled = computed(
  () => mutation.isPending.value === true || mutation.isSuccess.value === true,
);

const name = useField('name');
const interval = useField('interval');

//#endregion

//#region create/update

const queryClient = useQueryClient();
const mutation = useChoresApiUpsertChore(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    const payload = {
      name: values.name,
      interval: values.interval,
    };
    if (isEdit.value === true) payload.id = props.chore.id;
    await mutation.mutateAsync(payload);
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
    // TODO primevue confirm?
    close = confirm('There may be unsaved changes, are you sure you want to stop editing?');
  if (close) emit('close');
};
</script>

<style scoped lang="scss">
@import '@/styles/utilities';

form {
  @include form-styling;
}

:deep(.p-inputnumber-input) {
  text-align: center;
}
</style>
