import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    name : 'home',
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [{ path: '', component: () => import('pages/IndexPage.vue') }],
  },
  {
    name : 'login',
    path: '/login',
    component: () => import('layouts/MainLayout.vue'),
    children: [{ path: '', component: () => import('pages/Login.vue') }],
  },

  {
    path: '/people',
    component: () => import('layouts/MainLayout.vue'),
    children: [{ path: '', component: () => import('pages/People.vue') }],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
