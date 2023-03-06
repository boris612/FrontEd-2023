<template>
  <q-item v-if="isAuthenticated"
    clickable @click="handleLogout"
  >
    <q-item-section avatar>
      <q-icon name="logout" />
    </q-item-section>

    <q-item-section>
      <q-item-label>Odjava</q-item-label>
      <q-item-label caption>{{ fullname }}</q-item-label>
    </q-item-section>
  </q-item>

  <q-item v-if="!isAuthenticated" to="login" clickable>
  <q-item-section avatar>
      <q-icon name="login" />
    </q-item-section>

    <q-item-section>
      <q-item-label>Prijava</q-item-label>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { fetchItems } from 'src/services/api-client';
import { computed, onMounted, ref, watch} from 'vue';
import { getJwtToken, logout } from  'src/services/token-util';
import { useIsAuthenticatedStore } from 'src/stores/login-store';
import jwt_decode from "jwt-decode";

const isAuthenticated = computed(() => useIsAuthenticatedStore().isAuthenticated);
watch(isAuthenticated, async () => {
  if (isAuthenticated.value === true) await getUserInfo()
})

interface Claim {
  Type : string,
  Value: string
}

const claims = ref<Claim[]>()
const fullname = ref('')

onMounted( async () => {
  if (isAuthenticated.value === undefined || isAuthenticated.value === true) {
    getUserInfo()
  }
})

async function getUserInfo(){
  const token = getJwtToken();
  if (token !== null) {
    const tokenContent = jwt_decode(token) as {FullName : string};
    fullname.value = tokenContent.FullName
  }
  useIsAuthenticatedStore().storeisAuthenticated(token !== null)
}


async function handleLogout() {
  logout();
  fullname.value = '';
}

</script>
