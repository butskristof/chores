import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '@/pages/HomeView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/about',
      name: 'about',
      component: () => import('@/pages/AboutView.vue'),
    },
    {
      path: '/auth/unauthorized',
      name: 'auth.unauthorized',
      component: () => import('@/pages/auth/AuthUnauthorized.vue'),
    },
    {
      path: '/auth/oidc/sign-in',
      name: 'auth.oidc.sign-in',
      component: () => import('@/pages/auth/oidc/SignIn.vue'),
    },
    // {
    //   path: '/auth/oidc/silent-renew',
    //   name: 'auth.oidc.silent-renew',
    //   component: () => import('@/pages/auth/oidc/SilentRenew.vue'),
    // },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('@/pages/NotFound.vue'),
    },
  ],
});

export default router;
