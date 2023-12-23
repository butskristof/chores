/* eslint-env node */
require('@rushstack/eslint-patch/modern-module-resolution');

module.exports = {
  root: true,
  extends: [
    'plugin:vue/vue3-recommended',
    'eslint:all',
    'plugin:vue-scoped-css/vue3-recommended',
    '@vue/eslint-config-prettier/skip-formatting',
  ],
  parserOptions: {
    ecmaVersion: 'latest',
  },
  rules: {
    'no-restricted-imports': [
      'error',
      {
        patterns: ['../*', '^lodash$'],
      },
    ],
    'no-console': [
      process.env.NODE_ENV === 'production' ? 'error' : 'warn',
      {
        allow: ['error'],
      },
    ],
    'sort-imports': 'off',
    'sort-keys': 'off',
    'no-magic-numbers': 'off',
    'no-eq-null': 'off',
    eqeqeq: ['error', 'smart'],
    'one-var': ['error', 'never'],
    'id-length': 'off',
    'no-ternary': 'off',
    'spaced-comment': ['error', 'always', { markers: ['#region', '#endregion'] }],
    'max-lines': 'off',
    'capitalized-comments': 'off',
    'multiline-comment-style': ['error', 'separate-lines'],
    'no-warning-comments': 'off',
    'line-comment-position': 'off',
    'no-inline-comments': 'off',
    'no-plusplus': 'off',
    'no-use-before-define': 'off',
    'no-alert': 'off',
    'new-cap': 'off',
    'max-statements': 'off',
  },
};
