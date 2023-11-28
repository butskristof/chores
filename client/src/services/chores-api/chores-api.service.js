import { AuthenticatedApiService } from '@/services/utilities/authenticated-api.service';
import { accessTokenGetter } from '@/services/utilities/access-token-getter';
import { CHORES_API_BASEURL } from '@/utilities/env';

class ChoresApiService extends AuthenticatedApiService {
  constructor() {
    super(CHORES_API_BASEURL, accessTokenGetter);
  }
}

export default new ChoresApiService();
