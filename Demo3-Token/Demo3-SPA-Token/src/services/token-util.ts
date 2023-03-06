import { useIsAuthenticatedStore } from "src/stores/login-store";

const JWT_TOKEN = 'jwt';

export function storeToken(token : string) {
  localStorage.setItem(JWT_TOKEN, token);
}

export function getJwtToken() {
  return localStorage.getItem(JWT_TOKEN);
}


export function logout() {
  localStorage.removeItem(JWT_TOKEN);
  useIsAuthenticatedStore().storeisAuthenticated(false)
}

export function isAuthenticated() {
  const token = getJwtToken();
  return token ? true : false;
}
