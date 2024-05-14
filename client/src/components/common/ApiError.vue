<template>
  <div class="api-error">
    <PrimeButton
      class="toggle"
      :label="(showDetails ? 'Hide' : 'Show') + ' error details'"
      @click="showDetails = !showDetails"
    />
    <PrimeTabs
      v-if="showDetails"
      v-model:value="currentTab"
      class="error-details"
    >
      <PrimeTabList>
        <PrimeTab :value="TABS.API_RESPONSE">API response</PrimeTab>
        <PrimeTab :value="TABS.AXIOS_ERROR">Axios error</PrimeTab>
      </PrimeTabList>
      <PrimeTabPanels>
        <PrimeTabPanel :value="TABS.API_RESPONSE">
          <pre>{{ JSON.stringify(error.response.data, null, 2) }}</pre>
        </PrimeTabPanel>
        <PrimeTabPanel :value="TABS.AXIOS_ERROR">
          <pre>{{ JSON.stringify(error, null, 2) }}</pre>
        </PrimeTabPanel>
      </PrimeTabPanels>
    </PrimeTabs>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import PrimeButton from 'primevue/button';
import PrimeTabs from 'primevue/tabs';
import PrimeTabList from 'primevue/tablist';
import PrimeTab from 'primevue/tab';
import PrimeTabPanels from 'primevue/tabpanels';
import PrimeTabPanel from 'primevue/tabpanel';

defineProps({
  error: {
    type: Object,
    required: true,
  },
});

const showDetails = ref(false);

const TABS = {
  API_RESPONSE: 'API_RESPONSE',
  AXIOS_ERROR: 'AXIOS_ERROR',
};

const currentTab = TABS.API_RESPONSE;
</script>

<style scoped lang="scss">
@import '@/styles/_utilities.scss';

.error-details {
  margin-top: 1rem;
}

pre {
  @include pre-overflow;
}
</style>
