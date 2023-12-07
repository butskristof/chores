import { createRouter, createWebHistory } from 'vue-router';
import { routes } from '@/router/routes';
import authService from '@/services/auth/auth.service';
import { useAuthStore } from '@/stores/auth';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: routes.home.name,
      path: routes.home.path,
      redirect: {
        name: routes.chores.name,
      },
    },
    {
      name: routes.about.name,
      path: routes.about.path,
      component: () => import('@/pages/AboutView.vue'),
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
          component: () => import('@/pages/chores/ChoresOverview.vue'),
        },
        {
          name: routes.chores.children.detail.name,
          path: routes.chores.children.detail.path,
          component: () => import('@/pages/chores/ChoreDetail.vue'),
        },
      ],
    },
    {
      name: routes.tags.name,
      path: routes.tags.path,
      component: () => import('@/pages/tags/ManageTags.vue'),
    },
    {
      name: routes.auth.name,
      path: routes.auth.path,
      meta: {
        allowAnonymous: routes.auth.allowAnonymous,
      },
      children: [
        {
          name: routes.auth.children.unauthorized.name,
          path: routes.auth.children.unauthorized.path,
          meta: {
            allowAnonymous: routes.auth.children.unauthorized.allowAnonymous,
          },
          component: () => import('@/pages/auth/AuthUnauthorized.vue'),
        },
        {
          name: routes.auth.children.oidc.name,
          path: routes.auth.children.oidc.path,
          meta: {
            allowAnonymous: routes.auth.children.oidc.allowAnonymous,
          },
          children: [
            {
              name: routes.auth.children.oidc.children.signIn.name,
              path: routes.auth.children.oidc.children.signIn.path,
              meta: {
                allowAnonymous: routes.auth.children.oidc.children.signIn.allowAnonymous,
              },
              component: () => import('@/pages/auth/oidc/SignIn.vue'),
            },
            {
              name: routes.auth.children.oidc.children.silentRenew.name,
              path: routes.auth.children.oidc.children.silentRenew.path,
              meta: {
                allowAnonymous: routes.auth.children.oidc.children.silentRenew.allowAnonymous,
              },
              component: () => import('@/pages/auth/oidc/SilentRenew.vue'),
            },
          ],
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

router.beforeEach(async (to) => {
  if (to.matched.every((record) => record.meta.allowAnonymous === true)) {
    // no auth required, navigation may always happen
    return true;
  }
  // verify authentication, will return null if expired
  const user = await authService.getUser();
  if (user == null) {
    // not authenticated or expired, force into login flow
    // we pass in the (full) path so it can be used as a return path after
    // successful authentication
    // (full path so query and hash are preserved)
    authService.login(to.fullPath);
    return false; // explicitly cancel current navigation
  }
  // non-expired user is found
  // copy the user to the pinia store for easy access (it's made reactive this way)
  const authStore = useAuthStore();
  authStore.setUser(user);
  return true; // confirm the current navigation may happen
});

export default router;
