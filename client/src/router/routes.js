export const routes = {
  home: {
    name: 'home',
    path: '',
  },
  chores: {
    name: 'chores',
    path: 'chores',
    children: {
      overview: {
        name: 'chores.overview',
        path: '',
      },
      detail: {
        name: 'chores.detail',
        path: ':id',
      },
    },
  },
  tags: {
    name: 'tags',
    path: 'tags',
  },
  auth: {
    name: 'auth',
    path: '/auth',
    allowAnonymous: true,
    children: {
      unauthorized: {
        name: 'auth.unauthorized',
        path: 'unauthorized',
        allowAnonymous: true,
      },
      oidc: {
        name: 'auth.oidc',
        path: 'oidc',
        allowAnonymous: true,
        children: {
          signIn: {
            name: 'auth.oidc.sign-in',
            path: 'sign-in',
            allowAnonymous: true,
          },
          silentRenew: {
            name: 'auth.oidc.silent-renew',
            path: 'silent-renew',
            allowAnonymous: true,
          },
        },
      },
    },
  },
  notFound: {
    name: 'not-found',
    path: '/:pathMatch(.*)*',
  },
};
