/* eslint-disable camelcase */
import { OIDC_AUTHORITY, OIDC_CLIENT_ID, OIDC_LOGGING } from '@/utilities/env';
import { Log, UserManager } from 'oidc-client-ts';
import { stringIsNullOrWhitespace } from '@/utilities/string';
import { useAuthStore } from '@/stores/auth';

if (OIDC_LOGGING) {
  Log.setLogger(console);
  Log.setLevel(Log.DEBUG);
}

const oidcPath = `${window.location.origin}/auth/oidc`;

class AuthService {
  #userManager;

  constructor() {
    this.#userManager = this.#createUserManager();
  }

  #createUserManager = () => {
    const settings = {
      authority: OIDC_AUTHORITY,
      client_id: OIDC_CLIENT_ID,
      redirect_uri: `${oidcPath}/sign-in`,
      silent_redirect_uri: `${oidcPath}/silent-renew`,
      // TODO post logout redirect uri
      // TODO userStore
      // TODO load user info?

      // monitoring the session continuously (using a 'check session' iframe)
      // allows us to detect when a session is stopped elsewhere (e.g. another
      // tab) and to act on it by removing the user from local storage and
      // redirecting to the login screen
      monitorSession: true,
    };
    const userManager = new UserManager(settings);
    // this event is triggered when a silent renew delivers a new set of tokens
    userManager.events.addUserLoaded((user) => {
      // when we receive a new user, we update the information in the Pinia store
      // so we can always easily access the most recent information (and in effect
      // get the most recently collected access token)
      const store = useAuthStore();
      store.setUser(user);
    });

    // by enabling 'monitorSession' in the settings, we get notified of logouts
    // on the SSO (e.g. a logout in another client in another tab)
    // this way we can end this session immediately and redirect to the login screen
    userManager.events.addUserSignedOut(async () => {
      await userManager.removeUser();
      // if we're not in an iframe (e.g. on the SSO logout page), then we
      // call the login function to either authorize again or prompt for a new login
      if (window && window.top) {
        this.login(window.location.pathname + window.location.search);
      }
    });
    return userManager;
  };

  login = (returnPath = null, promptLogin = false) => {
    const args = {};
    if (!stringIsNullOrWhitespace(returnPath)) args.state = returnPath;
    if (promptLogin) args.prompt = 'login';
    return this.#userManager.signinRedirect(args);
  };
  callback = (silent = false) =>
    silent ? this.#userManager.signinSilentCallback() : this.#userManager.signinCallback();
  logout = (silent = false) =>
    silent ? this.#userManager.removeUser() : this.#userManager.signoutRedirect();
}

export default new AuthService();
