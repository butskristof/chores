<template>
  <PrimeInputGroup>
    <PrimeInputGroupAddon class="icon-display">
      <AppIcon
        v-if="model"
        :name="model"
      />
    </PrimeInputGroupAddon>
    <PrimeAutoComplete
      v-bind="$attrs"
      v-model="model"
      :input-id="inputId"
      :suggestions="filteredIcons"
      class="auto-complete"
      :virtual-scroller-options="{ itemSize: 38 }"
      @complete="searchIcon"
    >
      <template #option="{ option }">
        <div class="option">
          <AppIcon :name="option" />
          <span>{{ option }}</span>
        </div>
      </template>
    </PrimeAutoComplete>
  </PrimeInputGroup>
</template>

<script setup>
import PrimeAutoComplete from 'primevue/autocomplete';
import PrimeInputGroup from 'primevue/inputgroup';
import PrimeInputGroupAddon from 'primevue/inputgroupaddon';
import { ref } from 'vue';
import { ICON_NAMES } from '@/utilities/icons.js';
import { stringIsNullOrWhitespace } from '@/utilities/string.js';
import AppIcon from '@/components/common/AppIcon.vue';

defineProps({
  inputId: {
    type: String,
    default: () => null,
  },
});
const model = defineModel({
  type: String,
});

const filteredIcons = ref([...ICON_NAMES]);
const searchIcon = (event) => {
  const query = event.query?.trim().toLowerCase();
  if (stringIsNullOrWhitespace(query)) filteredIcons.value = [...ICON_NAMES];
  filteredIcons.value = ICON_NAMES.filter((i) => i.toLowerCase().includes(query));
};
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.icon-display {
  min-width: 50px;
}

.auto-complete {
  :deep(input) {
    width: 100%;
  }
}

.option {
  @include flex-row;
  align-items: center;
  gap: 1rem;
}
</style>
