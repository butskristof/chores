// eslint-disable-next-line no-promise-executor-return
export const sleep = (ms) => new Promise((r) => setTimeout(r, ms));
