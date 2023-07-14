import { createRouter, createWebHistory } from 'vue-router';
import { ROUTES } from '@/router/routes';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: {
        name: ROUTES.chores.name,
      },
    },
    {
      path: ROUTES.chores.path,
      name: ROUTES.chores.name,
      redirect: {
        name: ROUTES.chores.children.list.name,
      },
      children: [
        {
          path: ROUTES.chores.children.list.path,
          name: ROUTES.chores.children.list.name,
          component: () => import('@/pages/chores/ChoresPage.vue'),
        },
        {
          path: ROUTES.chores.children.detail.path,
          name: ROUTES.chores.children.detail.name,
          component: () => import('@/pages/chores/ChoreDetailPage.vue'),
        },
      ],
    },
  ],
});

export default router;
