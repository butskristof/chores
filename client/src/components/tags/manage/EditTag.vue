<template>
  <Dialog
    :visible="true"
    modal
    :draggable="false"
    :header="isEdit ? 'Edit tag' : 'Create new tag'"
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
      <div class="footer">
        <div class="result">
          <InlineMessage
            v-if="mutation.isSuccess.value === true"
            severity="success"
            >Tag saved</InlineMessage
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
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import { computed } from 'vue';
import { useField, useForm } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import { useChoresApiUpsertTag } from '@/composables/queries/chores-api.js';
import { useQueryClient } from '@tanstack/vue-query';
import InputText from 'primevue/inputtext';
import InlineMessage from 'primevue/inlinemessage';

const props = defineProps({
  tag: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);
const isEdit = computed(() => props.tag != null);

const queryClient = useQueryClient();

//#region form

const { handleSubmit, meta } = useForm({
  validationSchema: toTypedSchema(
    yup.object({
      name: yup.string().required().label('Name'),
    }),
  ),
  initialValues: {
    name: props.tag?.name,
  },
});

const isFormDisabled = computed(
  () => mutation.isPending.value === true || mutation.isSuccess.value === true,
);

const name = useField('name');

//#endregion

//#region create/update

const mutation = useChoresApiUpsertTag(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    const payload = {
      name: values.name,
    };
    if (isEdit.value === true) payload.id = props.tag.id;
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
