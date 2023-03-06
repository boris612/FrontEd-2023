import { defineStore } from 'pinia';

export const useIsAuthenticatedStore = defineStore('isAuthenticated', {
  state: () => ({
    isAuthenticated : undefined as boolean | undefined
  }),
  actions: {
    storeisAuthenticated(isAuthenticated : boolean) {
      this.isAuthenticated = isAuthenticated
    }
  },
});
