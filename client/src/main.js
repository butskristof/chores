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

import PrimeVue from 'primevue/config';
import ToastService from 'primevue/toastservice';
import DialogService from 'primevue/dialogservice';
import ConfirmationService from 'primevue/confirmationservice';

app.use(PrimeVue, {
  // ripple: true,
});
app.use(ToastService);
app.use(DialogService);
app.use(ConfirmationService);

import '@/styles/prime.scss';

//#region components

// import Menubar from 'primevue/menubar';
// // eslint-disable-next-line vue/multi-word-component-names
// app.component('Menubar', Menubar);
// import Avatar from 'primevue/avatar';
// // eslint-disable-next-line vue/multi-word-component-names
// app.component('Avatar', Avatar);
// import Menu from 'primevue/menu';
// // eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
// app.component('Menu', Menu);
// import Button from 'primevue/button';
// // eslint-disable-next-line vue/multi-word-component-names,vue/no-reserved-component-names
// app.component('Button', Button);

//#endregion

//#endregion

//#region toast
import 'vue-toastification/dist/index.css';
import Toast, { POSITION } from 'vue-toastification';
app.use(Toast, {
  position: POSITION.BOTTOM_RIGHT,
});
//#endregion

//#region uid

import { Uid } from '@shimyshack/uid';
app
  // .use(UidPlugin)
  .directive('uid', Uid);

//#endregion

app.mount('#app');
