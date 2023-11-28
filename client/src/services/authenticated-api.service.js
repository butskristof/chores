import axios from 'axios';
import { stringIsNullOrWhitespace } from '@/utilities/string';

const createAxiosInstance = (baseUrl, accessTokenGetter) => {
  const instance = axios.create({ baseURL: baseUrl });
  instance.interceptors.request.use((config) => {
    const accessToken = accessTokenGetter();
    if (!stringIsNullOrWhitespace(accessToken))
      config.headers.setAuthorization(`Bearer ${accessToken}`);
    return config;
  });
  return instance;
};

export class AuthenticatedApiService {
  #axiosInstance;

  constructor(baseUrl, accessTokenGetter) {
    this.#axiosInstance = createAxiosInstance(baseUrl, accessTokenGetter);
  }

  get(path, options = null) {
    return this.#axiosInstance.get(path, options);
  }

  post(path, data, options) {
    return this.#axiosInstance.post(path, data, options);
  }

  put(path, data, options) {
    return this.#axiosInstance.put(path, data, options);
  }

  delete(path, options = null) {
    return this.#axiosInstance.delete(path, options);
  }
}
