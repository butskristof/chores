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
    <form @submit.prevent="onSubmit">
      <div class="field">
        <label for="name">Name</label>
        <PrimeInputText
          id="name"
          type="text"
        />
      </div>

      <div class="footer">
        <div class="result">
          <p>Result can have a lot of content and we should wrap it correctly</p>
        </div>
        <div class="actions">
          <PrimeButton
            type="submit"
            label="Save"
            icon="pi pi-save"
          />
        </div>
      </div>
    </form>
  </PrimeDialog>
</template>

<script setup>
import { computed } from 'vue';
import PrimeDialog from 'primevue/dialog';
import PrimeInputText from 'primevue/inputtext';
import PrimeButton from 'primevue/button';

const props = defineProps({
  tag: {
    type: Object,
    default: () => null,
  },
});
const emit = defineEmits(['close']);
const isEdit = computed(() => props.tag != null);

//#region create/update

const onSubmit = () => {
  console.log('submit');
};

//#endregion

const updateVisible = (value) => {
  if (value === false) tryClose();
};
const tryClose = () => {
  emit('close');
};
</script>

<style scoped lang="scss">
@import '@/styles/_utilities';

form {
  .field {
    @include flex-column;
    gap: 0.5rem;

    margin-bottom: 1rem;
  }

  .footer {
    @include flex-row-justify-between-wrapping;
    flex-wrap: wrap-reverse;
  }
}
</style>
