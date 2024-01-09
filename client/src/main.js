import { createApp } from 'vue';
import App from './App.vue';
const app = createApp(App);

import '@/styles/reset.css';
import '@/styles/app.scss';

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
import PrimeVue from 'primevue/config';
app.use(PrimeVue, {
  ripple: true,
});

import 'primeicons/primeicons.css';
import 'primevue/resources/themes/lara-light-green/theme.css';
//#endregion

//#region uid

import { Uid } from '@shimyshack/uid';
app
  // .use(UidPlugin)
  .directive('uid', Uid);

//#endregion

app.mount('#app');
