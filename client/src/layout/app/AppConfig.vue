<template>
  <button
    v-if="false"
    class="layout-config-button p-link"
    type="button"
    @click="onConfigButtonClick"
  >
    <i class="pi pi-cog"></i>
  </button>

  <Sidebar
    v-model:visible="visible"
    position="right"
    :transition-options="'.3s cubic-bezier(0, 0, 0.2, 1)'"
    class="layout-config-sidebar"
  >
    <h5>Scale</h5>
    <div class="scale flex align-items-center">
      <Button
        icon="pi pi-minus"
        type="button"
        class="p-button-text p-button-rounded w-2rem h-2rem mr-2"
        :disabled="layoutConfig.scale.value === scales[0]"
        @click="decrementScale()"
      ></Button>
      <div class="flex gap-2 align-items-center">
        <i
          v-for="s in scales"
          :key="s"
          class="pi pi-circle-fill text-300"
          :class="{ 'text-primary-500': s === layoutConfig.scale.value }"
        ></i>
      </div>
      <Button
        icon="pi pi-plus"
        type="button"
        p-button
        class="p-button-text p-button-rounded w-2rem h-2rem ml-2"
        :disabled="layoutConfig.scale.value === scales[scales.length - 1]"
        @click="incrementScale()"
      ></Button>
    </div>

    <h5>Menu Type</h5>
    <div class="flex">
      <div class="field-radiobutton flex-1">
        <RadioButton
          v-model="layoutConfig.menuMode.value"
          name="menuMode"
          value="static"
          input-id="mode1"
        ></RadioButton>
        <label for="mode1">Static</label>
      </div>

      <div class="field-radiobutton flex-1">
        <RadioButton
          v-model="layoutConfig.menuMode.value"
          name="menuMode"
          value="overlay"
          input-id="mode2"
        ></RadioButton>
        <label for="mode2">Overlay</label>
      </div>
    </div>

    <h5>Input Style</h5>
    <div class="flex">
      <div class="field-radiobutton flex-1">
        <RadioButton
          v-model="layoutConfig.inputStyle.value"
          name="inputStyle"
          value="outlined"
          input-id="outlined_input"
        ></RadioButton>
        <label for="outlined_input">Outlined</label>
      </div>
      <div class="field-radiobutton flex-1">
        <RadioButton
          v-model="layoutConfig.inputStyle.value"
          name="inputStyle"
          value="filled"
          input-id="filled_input"
        ></RadioButton>
        <label for="filled_input">Filled</label>
      </div>
    </div>

    <h5>Ripple Effect</h5>
    <InputSwitch v-model="layoutConfig.ripple.value"></InputSwitch>

    <h5>PrimeOne Design - 2022</h5>
    <div class="grid">
      <div class="col-3">
        <button
          class="p-link w-2rem h-2rem"
          @click="onChangeTheme('lara-light-indigo', 'light')"
        >
          <img
            src="/layout/images/themes/lara-light-indigo.png"
            class="w-2rem h-2rem"
            alt="Lara Light Indigo"
          />
        </button>
      </div>
      <div class="col-3">
        <button
          class="p-link w-2rem h-2rem"
          @click="onChangeTheme('lara-dark-indigo', 'dark')"
        >
          <img
            src="/layout/images/themes/lara-dark-indigo.png"
            class="w-2rem h-2rem"
            alt="Lara Dark Indigo"
          />
        </button>
      </div>
    </div>
  </Sidebar>
</template>

<script setup>
import RadioButton from 'primevue/radiobutton';
import Button from 'primevue/button';
import InputSwitch from 'primevue/inputswitch';
import Sidebar from 'primevue/sidebar';

import { ref } from 'vue';
import { useLayout } from '@/layout/app/composables/layout.js';

const scales = ref([12, 13, 14, 15, 16]);
const visible = ref(false);

const { changeThemeSettings, setScale, layoutConfig } = useLayout();

const onConfigButtonClick = () => {
  visible.value = !visible.value;
};
const onChangeTheme = (theme, mode) => {
  const elementId = 'theme-css';
  const linkElement = document.getElementById(elementId);
  const cloneLinkElement = linkElement.cloneNode(true);
  const newThemeUrl = linkElement.getAttribute('href').replace(layoutConfig.theme.value, theme);
  cloneLinkElement.setAttribute('id', `${elementId}-clone`);
  cloneLinkElement.setAttribute('href', newThemeUrl);
  cloneLinkElement.addEventListener('load', () => {
    linkElement.remove();
    cloneLinkElement.setAttribute('id', elementId);
    changeThemeSettings(theme, mode === 'dark');
  });
  linkElement.parentNode.insertBefore(cloneLinkElement, linkElement.nextSibling);
};
const decrementScale = () => {
  setScale(layoutConfig.scale.value - 1);
  applyScale();
};
const incrementScale = () => {
  setScale(layoutConfig.scale.value + 1);
  applyScale();
};
const applyScale = () => {
  document.documentElement.style.fontSize = `${layoutConfig.scale.value}px`;
};
</script>
