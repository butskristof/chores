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

app.use(VueQueryPlugin);

//#endregion

//#region primevue

import 'primeicons/primeicons.css';
import PrimeVue from 'primevue/config';
import Aura from 'primevue/themes/aura';
import Ripple from 'primevue/ripple';

app.use(PrimeVue, {
  theme: {
    preset: Aura,
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

//#region multiselect

// import 'vue-multiselect/dist/vue-multiselect.css';

//#endregion

//#region uid

// import { Uid } from '@shimyshack/uid';
//
// app
//   // .use(UidPlugin)
//   .directive('uid', Uid);

//#endregion

app.mount('#app');
