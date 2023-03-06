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
import { fetchItems, logout } from 'src/services/api-client';
import { computed, onMounted, ref, watch} from 'vue';
import { useIsAuthenticatedStore } from 'src/stores/login-store';

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
  claims.value = await fetchItems('account/info');
  if (claims.value.length > 0) {
    fullname.value = claims.value.find(c => c.Type === 'FullName')?.Value ?? ''
  }
  useIsAuthenticatedStore().storeisAuthenticated(claims.value.length > 0)
}


async function handleLogout() {
  const ok = await logout();
  if (ok) {
    fullname.value = '';
    useIsAuthenticatedStore().storeisAuthenticated(false)
  }
}

</script>
