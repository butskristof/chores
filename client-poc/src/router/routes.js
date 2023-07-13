export const ROUTES = {
  chores: {
    path: '/chores',
    name: 'chores',
    children: {
      list: {
        path: '',
        name: 'chores.list',
      },
      detail: {
        path: ':id',
        name: 'chores.detail',
      },
    },
  },
};
