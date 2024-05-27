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
  notFound: {
    name: 'not-found',
    path: '/:pathMatch(.*)*',
  },
};
