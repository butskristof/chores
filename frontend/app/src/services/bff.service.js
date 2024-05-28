import { AuthenticatedApiService } from '@/services/authenticated-api.service.js';

class BffService extends AuthenticatedApiService {
  constructor() {
    super('/bff');
  }

  getUser = () => this.get('/user').then((r) => r.data);
  getDiagnostics = () => this.get('/diagnostics').then((r) => r.data);
}

export default new BffService();
