/* eslint-disable no-undef */
import { fileURLToPath, URL } from 'node:url';

import { defineConfig, loadEnv } from 'vite';
import vue from '@vitejs/plugin-vue';
import eslint from 'vite-plugin-eslint';

export default ({ mode }) => {
  // the env isn't readily available, so we first create an object using Node's environment
  // and merge Vite's environment into it
  const env = { ...process.env, ...loadEnv(mode, process.cwd()) };

  // while developing the /api, /bff and /signin-oidc routes will be proxied to the BFF
  // when deployed, the BFF will serve the SPA assets and have these routes on the host itself
  // this way, we can emulate running the BFF and SPA running on the same host while using the
  // dev server for local development
  // changing the origin will make the BFF believe the dev server is running on the same origin,
  // but keep in mind it's including Secure cookies over http because it's localhost
  const BFF_PATHS = ['/api', '/bff', '/signin-oidc'];
  const bffProxy = BFF_PATHS.reduce((config, path) => {
    config[path] = {
      target: env.VITE_CHORES_BFF_BASEURL,
      secure: false, // don't verify the BFF's certificate (it'll be self-signed)
      changeOrigin: true,
    };
    return config;
  }, {});

  // https://vitejs.dev/config/
  return defineConfig({
    plugins: [vue(), eslint()],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
    },
    server: {
      proxy: {
        ...bffProxy,
      },
    },
  });
};
