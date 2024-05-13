import { createRouter, createWebHistory } from 'vue-router';
import { routes } from '@/router/routes';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      children: [
        {
          name: routes.home.name,
          path: routes.home.path,
          redirect: {
            name: routes.chores.name,
          },
        },
        {
          name: routes.chores.name,
          path: routes.chores.path,
          redirect: {
            name: routes.chores.children.overview.name,
          },
          children: [
            {
              name: routes.chores.children.overview.name,
              path: routes.chores.children.overview.path,
              component: () => import('@/pages/app/chores/ChoresOverview.vue'),
            },
            {
              name: routes.chores.children.detail.name,
              path: routes.chores.children.detail.path,
              component: () => import('@/pages/app/chores/ChoreDetail.vue'),
            },
          ],
        },
        {
          name: routes.tags.name,
          path: routes.tags.path,
          component: () => import('@/pages/app/tags/ManageTags.vue'),
        },
      ],
    },
    {
      name: routes.notFound.name,
      path: routes.notFound.path,
      component: () => import('@/pages/NotFound.vue'),
    },
  ],
});

export default router;
