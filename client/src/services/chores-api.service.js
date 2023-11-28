import { AuthenticatedApiService } from '@/services/authenticated-api.service';
import { accessTokenGetter } from '@/services/utilities/access-token-getter';
import { CHORES_API_BASEURL } from '@/utilities/env';

class ChoresApiService extends AuthenticatedApiService {
  constructor() {
    super(CHORES_API_BASEURL, accessTokenGetter);
  }

  //#region tags

  getTags = () => this.get('/tags').then((r) => r.data);

  //#endregion
}

export default new ChoresApiService();
