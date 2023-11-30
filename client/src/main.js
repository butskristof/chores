import { createApp } from 'vue';
import App from './App.vue';
const app = createApp(App);

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

//#region tippy
// import VueTippy from 'vue-tippy';
import 'tippy.js/dist/tippy.css';
// app.use(VueTippy, {
//   component: 'tippy',
// });
//#endregion

app.mount('#app');
