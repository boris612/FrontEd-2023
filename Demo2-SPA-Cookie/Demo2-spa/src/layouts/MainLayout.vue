<template>
  <q-layout view="hhh lpR fFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn flat dense round icon="menu" aria-label="Menu"  @click="toggleLeftDrawer" />

        <q-toolbar-title>
          Demo 2
        </q-toolbar-title>

      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered >
      <q-list>
        <q-item-label header>
          <LoginStatus />
        </q-item-label>

        <EssentialLink
          v-for="link in links"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import EssentialLink, { EssentialLinkProps } from 'components/EssentialLink.vue';
import LoginStatus from 'components/LoginStatus.vue'
const leftDrawerOpen = ref(false)

const links : EssentialLinkProps[]  = [
  {
    title: 'Početna',
    caption: '',
    icon: 'home',
    link: '/'
  },
  {
    title: 'Osobe',
    caption: 'Popis osoba',
    icon: 'supervisor_account',
    link: '/people'
  },
];

function toggleLeftDrawer () {
  leftDrawerOpen.value = !leftDrawerOpen.value
}

</script>
