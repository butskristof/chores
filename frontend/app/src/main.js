//#region styling

import '@/styles/reset.css';
import '@/styles/main.scss';

//#endregion

import { createApp } from 'vue';
import App from './App.vue';

const app = createApp(App);

//#region router

import router from './router';
app.use(router);

//#endregion

//#region pinia

import { createPinia } from 'pinia';
app.use(createPinia());

//#endregion

//#region tanstack query

import { VueQueryPlugin } from '@tanstack/vue-query';
app.use(VueQueryPlugin, {
  enableDevtoolsV6Plugin: true,
});

//#endregion

//#region primevue

import 'primeicons/primeicons.css';
import PrimeVue from 'primevue/config';
import Aura from 'primevue/themes/aura';
import Ripple from 'primevue/ripple';
import { definePreset } from 'primevue/themes';

const preset = definePreset(Aura, {
  semantic: {
    colorScheme: {
      light: {
        surface: {
          0: '#ffffff',
          50: '{slate.50}',
          100: '{slate.100}',
          200: '{slate.200}',
          300: '{slate.300}',
          400: '{slate.400}',
          500: '{slate.500}',
          600: '{slate.600}',
          700: '{slate.700}',
          800: '{slate.800}',
          900: '{slate.900}',
          950: '{slate.950}',
        },
      },
      dark: {
        surface: {
          0: '#ffffff',
          50: '{zinc.50}',
          100: '{zinc.100}',
          200: '{zinc.200}',
          300: '{zinc.300}',
          400: '{zinc.400}',
          500: '{zinc.500}',
          600: '{zinc.600}',
          700: '{zinc.700}',
          800: '{zinc.800}',
          900: '{zinc.900}',
          950: '{zinc.950}',
        },
      },
    },
  },
});

app.use(PrimeVue, {
  theme: {
    preset,
    options: {
      darkModeSelector: 'html[color-scheme=dark]',
    },
  },
});
app.directive('ripple', Ripple);

//#endregion

//#region toast

import 'vue-toastification/dist/index.css';
import Toast, { POSITION } from 'vue-toastification';
app.use(Toast, {
  position: POSITION.BOTTOM_RIGHT,
});

//#endregion

//#region tippy

// import VueTippy from 'vue-tippy';
import 'tippy.js/dist/tippy.css';
// app.use(VueTippy, {
//   component: 'tippy',
// });

//#endregion

//#region icons

import { addIcons } from 'oh-vue-icons';
import { ICONS } from '@/utilities/icons.js';

addIcons(...ICONS);

//#endregion

//#region quill

import 'quill/dist/quill.core.css';
// the editor itself is wrapped by PrimeVue, so we don't have to register
// or configure it here
// importing the styles into the app is necessary to correctly show
// Quill-formatted markup

//#endregion

//#region dompurify
import VueDOMPurifyHTML from 'vue-dompurify-html';
app.use(VueDOMPurifyHTML);
//#endregion

app.mount('#app');
