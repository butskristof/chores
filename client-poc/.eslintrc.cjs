/* eslint-env node */
require('@rushstack/eslint-patch/modern-module-resolution')

module.exports = {
  root: true,
  env: {
    browser: true,
    node: true,
  },
  'extends': [
    'eslint:all',
    'plugin:vue/vue3-recommended',
    'plugin:vue-scoped-css/vue3-recommended',
    '@vue/eslint-config-prettier/skip-formatting'
  ],
  parserOptions: {
    ecmaVersion: 'latest'
  },
  rules: {
    'no-restricted-imports': [
      'error',
      {
        patterns: ['../*'],
      },
    ],
    'no-console': process.env.NODE_ENV === 'production' ? 'error' : 'warn',
    'sort-imports': 'off',
    'sort-keys': 'off',
    // 'capitalized-comments': 'off',
    // 'no-magic-numbers': 'off', // TODO evaluate
    // 'one-var': 'off',
    // 'multiline-comment-style': ['error', 'separate-lines'],
  },
}
