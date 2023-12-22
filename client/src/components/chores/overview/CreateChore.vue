<template>
  <AppDialog
    :open="open"
    @close="tryClose"
  >
    <div
      v-if="mutation.isSuccess.value === true"
      class="success"
    >
      <p>Chore was created successfully.</p>
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
      <DialogTitle>Create new chore</DialogTitle>
      <form @submit="save">
        <TextInput
          label="Name"
          name="name"
          :disabled="formDisabled"
        />

        <TextInput
          name="interval"
          label="Interval"
          type="number"
        />

        <div class="actions">
          <button
            type="submit"
            :disabled="formDisabled"
          >
            <span v-if="mutation.isPending.value === true">Saving...</span>
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
import TextInput from '@/components/common/form/inputs/TextInput.vue';
import { useForm } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import { computed } from 'vue';
import { useQueryClient } from '@tanstack/vue-query';
import { useChoresApiCreateChore } from '@/composables/queries/chores-api';
import { useToast } from 'vue-toastification';

defineProps({
  open: {
    type: Boolean,
    default: false,
  },
});
const emit = defineEmits(['close']);

const queryClient = useQueryClient();
const toast = useToast();

//#region form

const { handleSubmit, meta } = useForm({
  validationSchema: toTypedSchema(
    yup.object({
      name: yup.string().required().label('Name'),
      interval: yup.number().required().positive().integer().label('Interval'),
    }),
  ),
});
const formDisabled = computed(
  () => mutation.isPending.value === true || mutation.isSuccess.value === true,
);

//#endregion

//#region create

const mutation = useChoresApiCreateChore(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    await mutation.mutateAsync(values);
    toast.success('Chore created');
    tryClose(true);
  } catch (e) {
    console.error(e);
  }
});

const tryClose = (force = false) => {
  let close = true;
  if (!force && meta.value.dirty)
    close = confirm('There may be unsaved changes, are you sure you want to stop editing?');
  if (close) emit('close');
};

//#endregion
</script>

<style scoped lang="scss">
.actions {
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
}
</style>
