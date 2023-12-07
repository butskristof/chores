<template>
  <div class="form-input">
    <label
      :for="input?.id"
      class="left"
      >{{ label }}</label
    >
    <div class="right">
      <input
        ref="input"
        v-model="value"
        v-uid
        :type="type"
        :class="{ invalid: meta.touched && !meta.valid }"
        :disabled="disabled"
      />
      <div
        v-if="errorMessage"
        class="error"
      >
        {{ errorMessage }}
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useField } from 'vee-validate';

const props = defineProps({
  label: {
    type: String,
    required: true,
  },
  name: {
    type: String,
    required: true,
  },
  type: {
    type: String,
    default: 'text',
  },
  disabled: { type: Boolean, default: false },
});

const input = ref();
// eslint-disable-next-line no-undefined
const { value, errorMessage, meta } = useField(() => props.name, undefined);
</script>

<style scoped lang="scss">
.form-input {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;

  label {
    flex-basis: 25%;
    flex-shrink: 1;
    padding-top: 0.25rem;
  }

  .right {
    flex-grow: 1;

    input {
      display: block;
      width: 100%;
    }

    .error {
      width: 100%;
      color: red;
      font-weight: bold;
    }
  }
}

input.invalid {
  border: 1px solid red;
}
</style>
