<template>
  <Dialog
    :visible="visible"
    modal
    :header="isEdit ? 'Edit tag' : 'Create new tag'"
    @update:visible="updateVisible"
  >
    <form @submit="save">
      <div class="field">
        <label for="name">Name</label>
        <InputText
          id="name"
          v-model="name.value.value"
          type="text"
          :disabled="formDisabled"
        />
      </div>
      <div class="actions">
        <Button
          type="submit"
          :label="isEdit ? 'Create' : 'Save changes'"
          icon="pi pi-save"
          :disabled="formDisabled"
          :loading="mutation.isPending.value"
        />
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

const props = defineProps({
  tag: {
    type: Object,
    default: () => null,
  },
});
const visible = defineModel('visible', { type: Boolean });
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

const formDisabled = computed(
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
    // toast.success(isEdit.value === true ? 'Tag updated' : 'Tag created');
    // tryClose(true);
  } catch (e) {
    console.error(e);
  }
});

//#endregion

const updateVisible = (value) => {
  if (value === true) visible.value = true;
  else tryClose();
};
const tryClose = (force = false) => {
  let close = true;
  if (!force && meta.value.dirty)
    close = confirm('There may be unsaved changes, are you sure you want to stop editing?');
  if (close) visible.value = false;
};
</script>

<style scoped lang="scss">
form {
  .field {
    label {
      display: block;
    }
  }
  .actions {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
  }
}
</style>
