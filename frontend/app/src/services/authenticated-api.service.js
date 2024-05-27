import axios from 'axios';

const createAxiosInstance = (baseUrl) => axios.create({ baseURL: baseUrl });

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
