<template>
  <AppDialog
    :open="open"
    @close="emit('close')"
  >
    <div
      v-if="mutation.isSuccess.value === true"
      class="success"
    >
      <p>Tag was {{ isEdit ? 'updated' : 'created' }} successfully.</p>
      <div class="actions">
        <button
          type="button"
          @click="emit('close')"
        >
          Close
        </button>
      </div>
    </div>
    <template v-else>
      <DialogTitle>
        {{ isEdit ? 'Edit tag' : 'Create new tag' }}
      </DialogTitle>
      <form @submit="save">
        <TextInput
          label="Name"
          name="name"
          :disabled="formDisabled"
        />

        <div
          v-if="!mutation.isSuccess.value === true"
          class="actions"
        >
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
import { useToast } from 'vue-toastification';
import { useChoresApiUpsertTag } from '@/composables/queries/chores-api';
import { useQueryClient } from '@tanstack/vue-query';
import TextInput from '@/components/common/form/inputs/TextInput.vue';
import { useForm } from 'vee-validate';
import * as yup from 'yup';
import { toTypedSchema } from '@vee-validate/yup';
import { DialogTitle } from '@headlessui/vue';
import { computed } from 'vue';

const props = defineProps({
  open: {
    type: Boolean,
    default: false,
  },
  tag: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);

const queryClient = useQueryClient();
const toast = useToast();

const isEdit = computed(() => props.tag != null);

//#region form

const { handleSubmit } = useForm({
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

//#endregion

//#region create/update

const mutation = useChoresApiUpsertTag(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    const payload = {
      name: values.name,
      choresCount: Math.floor(Math.random() * 3),
    };
    if (isEdit.value === true) payload.id = props.tag.id;
    await mutation.mutateAsync(payload);
    toast.success(isEdit.value === true ? 'Tag updated' : 'Tag created');
    emit('close');
  } catch (e) {
    console.error(e);
  }
});

//#endregion
</script>

<style scoped lang="scss">
.actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
}
</style>
