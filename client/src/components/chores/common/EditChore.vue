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
        <label for="name">Name</label>
        <InputText
          id="name"
          v-model.trim="name.value.value"
          autofocus
          type="text"
          :disabled="isFormDisabled"
          :class="{ 'p-invalid': name.errorMessage.value }"
        />
        <small
          v-if="name.errorMessage"
          class="p-error"
          >{{ name.errorMessage }}</small
        >
      </div>

      <div class="field">
        <label for="interval">Interval</label>
        <InputText
          id="interval"
          v-model.trim="interval.value.value"
          autofocus
          type="number"
          :disabled="isFormDisabled"
          :class="{ 'p-invalid': interval.errorMessage.value }"
        />
        <small
          v-if="interval.errorMessage"
          class="p-error"
          >{{ interval.errorMessage }}</small
        >
      </div>

      <div class="footer">
        <div class="result">
          <InlineMessage
            v-if="mutation.isSuccess.value === true"
            severity="success"
            >Chore saved</InlineMessage
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
import { computed } from 'vue';
import { useField, useForm } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import { useChoresApiUpsertChore } from '@/composables/queries/chores-api';
import { useQueryClient } from '@tanstack/vue-query';

const props = defineProps({
  chore: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);
const isEdit = computed(() => props.chore != null);

const queryClient = useQueryClient();

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

const mutation = useChoresApiUpsertChore(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    const payload = { ...values };
    if (isEdit.value === true) payload.id = props.chore.id;
    await mutation.mutateAsync(payload);
    // toast.success(isEdit.value === true ? 'Chore updated' : 'Chore created');
    // tryClose(true);
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
