<template>
  <q-item v-if="isAuthenticated"
    clickable @click="handleLogout"
  >
    <q-item-section avatar>
      <q-icon name="logout" />
    </q-item-section>

    <q-item-section>
      <q-item-label>Odjava</q-item-label>
      <q-item-label caption>{{ user.name  }}</q-item-label>
    </q-item-section>
  </q-item>

  <q-item v-if="!isAuthenticated" @click="handleLogin" clickable>
  <q-item-section avatar>
      <q-icon name="login" />
    </q-item-section>

    <q-item-section>
      <q-item-label>Prijava</q-item-label>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { useAuth0 } from "@auth0/auth0-vue";


const { isLoading, loginWithRedirect, logout, isAuthenticated, user  } = useAuth0();

const handleLogin = () => {
  loginWithRedirect({
    appState: {
       target: "/profile",
    },
  });
};

const handleLogout = () =>
  logout({
    logoutParams: {
      returnTo: window.location.origin,
    }
  });
</script>
