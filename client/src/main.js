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

app.mount('#app');
