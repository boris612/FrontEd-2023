import { createAuth0 } from '@auth0/auth0-vue';
import { boot } from 'quasar/wrappers';

export const authPlugin = createAuth0({
  domain: 'fer-web2.eu.auth0.com',
  clientId: 'Ba9cbvvJ3s6IaCyWygFokFomhk38Qv5K',
  cacheLocation: 'localstorage',
  authorizationParams: {
    redirect_uri: window.location.origin,
    audience: "https://fronted/demo",
    scope: 'email profile'
  }
})

export default boot(({ app }) => {
  app.use(authPlugin)
})
