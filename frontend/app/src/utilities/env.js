// these getters try to retrieve a value from either the config.js file (generated on
// container startup to support runtime environment variables), or the 'normal' dotenv
// environment during development
const getEnv = (name) => window?.configs?.[name] ?? import.meta.env[name];
// const getEnvBool = (name) => getEnv(name) === 'true';

export const CHORES_API_BASEURL = getEnv('VITE_CHORES_API_BASEURL');
export const CHORES_BFF_BASEURL = getEnv('VITE_CHORES_BFF_BASEURL');
