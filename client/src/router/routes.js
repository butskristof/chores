export const routes = {
  home: {
    name: 'home',
    path: '/',
  },
  about: {
    name: 'about',
    path: '/about',
  },
  tags: {
    name: 'tags',
    path: '/tags',
    children: {
      overview: {
        name: 'tags.overview',
        path: '',
      },
    },
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
