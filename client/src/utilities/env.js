// these getters try to retrieve a value from either the config.js file (generated on
// container startup to support runtime environment variables), or the 'normal' dotenv
// environment during development
const getEnv = (name) => window?.configs?.[name] ?? import.meta.env[name];
const getEnvBool = (name) => getEnv(name) === 'true';

export const OIDC_AUTHORITY = getEnv('VITE_OIDC_AUTHORITY');
export const OIDC_CLIENT_ID = getEnv('VITE_OIDC_CLIENT_ID');
export const OIDC_LOGGING = getEnvBool('VITE_OIDC_LOGGING');

export const CHORES_API_BASEURL = getEnv('VITE_CHORES_API_BASEURL');
