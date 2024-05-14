<template>
  <PrimeDialog
    :visible="true"
    modal
    maximizable
    :draggable="false"
    :header="isEdit ? 'Edit tag' : 'Create new tag'"
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

      <div class="row">
        <div class="field">
          <label for="color">Color</label>
          <PrimeInputText
            id="color"
            v-model.trim="color.value.value"
            type="color"
            class="color-picker"
            :disabled="isFormDisabled"
            :invalid="color.errors.value.length > 0"
          />
          <PrimeInputText
            v-model.trim="color.value.value"
            type="text"
            :disabled="isFormDisabled"
            :invalid="color.errors.value.length > 0"
          />
          <small
            v-if="color.errors.value.length > 0"
            class="p-error"
          >
            <span v-if="color.errors.value.length === 1">{{ color.errorMessage.value }}</span>
            <template v-else>
              <span
                v-for="error in color.errors.value"
                :key="error"
                >{{ error }} <br
              /></span>
            </template>
          </small>
        </div>
        <div class="field">
          <label for="icon">Icon</label>
          <PrimeInputGroup>
            <PrimeInputGroupAddon>
              <i :class="icon.value.value" />
            </PrimeInputGroupAddon>
            <PrimeAutoComplete
              id="icon"
              v-model="icon.value.value"
              :suggestions="filteredIcons"
              :disabled="isFormDisabled"
              :invalid="icon.errors.value.length > 0"
              @complete="searchIcon"
            >
              <template #option="slotProps">
                <div class="option">
                  <i :class="slotProps.option" />
                  {{ slotProps.option }}
                </div>
              </template>
            </PrimeAutoComplete>
          </PrimeInputGroup>
          <PrimeInputText
            v-if="false"
            v-model="icon.value.value"
            type="text"
            :disabled="isFormDisabled"
            :invalid="icon.errors.value.length > 0"
          />
          <small
            v-if="icon.errors.value.length > 0"
            class="p-error"
          >
            <span v-if="icon.errors.value.length === 1">{{ icon.errorMessage.value }}</span>
            <template v-else>
              <span
                v-for="error in icon.errors.value"
                :key="error"
                >{{ error }} <br
              /></span>
            </template>
          </small>
        </div>
      </div>

      <div class="footer">
        <div class="result">
          <PrimeInlineMessage
            v-if="mutation.isSuccess.value"
            severity="success"
            >Tag saved</PrimeInlineMessage
          >
          <ApiError
            v-if="mutation.isError.value"
            :error="mutation.error.value"
          >
            <template #message>
              <p>
                Saving the tag failed, please refer to the error information below for more details.
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
import { computed, ref } from 'vue';
import PrimeDialog from 'primevue/dialog';
import PrimeInputText from 'primevue/inputtext';
import PrimeButton from 'primevue/button';
import PrimeInlineMessage from 'primevue/inlinemessage';
import { useField, useForm } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import { useChoresApiUpsertTag } from '@/composables/queries/chores-api.js';
import { useQueryClient } from '@tanstack/vue-query';
import ApiError from '@/components/common/ApiError.vue';
import PrimeAutoComplete from 'primevue/autocomplete';
import { PrimeIcons } from 'primevue/api';
import { stringIsNullOrWhitespace } from '@/utilities/string.js';
import PrimeInputGroup from 'primevue/inputgroup';
import PrimeInputGroupAddon from 'primevue/inputgroupaddon';

const props = defineProps({
  tag: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);
const isEdit = computed(() => props.tag != null);

//#region form

const { handleSubmit, meta } = useForm({
  validationSchema: toTypedSchema(
    yup.object({
      name: yup.string().required().label('Name'),
      color: yup
        .string()
        .matches(/^#(([0-9a-fA-F]{2}){3})$/u, 'Color must be a valid hex code (e.g. #ffffff)')
        .label('Color'),
      icon: yup.string().max(32).label('Icon'),
    }),
  ),
  initialValues: {
    name: props.tag?.name,
    color: props.tag?.color ?? '#ffffff',
    icon: props.tag?.icon,
  },
});

const isFormDisabled = computed(
  () => mutation.isPending.value === true || mutation.isSuccess.value === true,
);

const name = useField('name');
const color = useField('color');
const icon = useField('icon');

//#region icon
const icons = Object.values(PrimeIcons);
const filteredIcons = ref(Object.values(icons));
const searchIcon = (event) => {
  if (stringIsNullOrWhitespace(event.query)) filteredIcons.value = [...icons];
  else {
    const query = event.query.trim().toLowerCase();
    filteredIcons.value = icons.filter((i) => i.toLowerCase().includes(query));
  }
};
//#endregion

//#endregion

//#region create/update

const queryClient = useQueryClient();
const mutation = useChoresApiUpsertTag(queryClient);

const save = handleSubmit.withControlled(async (values) => {
  try {
    const payload = {
      name: values.name,
      color: values.color,
      icon: values.icon,
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
    // TODO primevue confirm?
    close = confirm('There may be unsaved changes, are you sure you want to stop editing?');
  if (close) emit('close');
};
</script>

<style scoped lang="scss">
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';

form {
  .field {
    @include flex-column;
    gap: 0.5rem;

    margin-bottom: 1rem;
  }

  .row {
    @include flex-column;
    @include media-min-width($sm) {
      @include flex-row;
      gap: var(--default-padding);

      > * {
        flex-basis: 50%;
      }
    }
  }

  .footer {
    @include flex-row-justify-between-wrapping;
    align-items: flex-end;
    flex-wrap: wrap-reverse;

    .result {
      max-width: 100%;
    }
  }
}

.color-picker {
  width: 100%;
  line-height: 1.5rem;
  height: 3rem;
}

.option {
  @include flex-row;
  align-items: center;
  gap: 0.5rem;

  :deep(input) {
    width: 100%;
  }
}
</style>
