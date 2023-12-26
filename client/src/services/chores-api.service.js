import { AuthenticatedApiService } from '@/services/authenticated-api.service';
import { accessTokenGetter } from '@/services/utilities/access-token-getter';
import { CHORES_API_BASEURL } from '@/utilities/env';

class ChoresApiService extends AuthenticatedApiService {
  constructor() {
    super(CHORES_API_BASEURL, accessTokenGetter);
  }

  //#region chores

  getChores = () => this.get('/chores').then((r) => r.data);
  createChore = (payload) => this.post('/chores', payload).then((r) => r.data);
  updateChore = (payload) => this.put(`/chores/${payload.id}`, payload).then((r) => r.data);
  getChore = (id) => this.get(`/chores/${id}`).then((r) => r.data);
  deleteChore = (id) => this.delete(`/chores/${id}`).then((r) => r.data);
  updateChoreNotes = (payload) =>
    this.put(`/chores/${payload.choreId}/notes`, payload).then((r) => r.data);
  updateChoreTags = (payload) =>
    this.put(`/chores/${payload.choreId}/tags`, payload).then((r) => r.data);
  createChoreIteration = (payload) =>
    this.post(`/chores/${payload.choreId}/iterations`, payload).then((r) => r.data);
  updateChoreIteration = (payload) =>
    this.put(`/chores/${payload.choreId}/iterations/${payload.iterationId}`, payload).then(
      (r) => r.data,
    );
  deleteChoreIteration = ({ choreId, iterationId }) =>
    this.delete(`/chores/${choreId}/iterations/${iterationId}`).then((r) => r.data);

  //#endregion

  //#region tags

  getTags = () => this.get('/tags').then((r) => r.data);

  createTag = (payload) => this.post('/tags', payload).then((r) => r.data);

  updateTag = (payload) => this.put(`/tags/${payload.id}`, payload).then((r) => r.data);

  deleteTag = (id) => this.delete(`/tags/${id}`);

  //#endregion
}

export default new ChoresApiService();
