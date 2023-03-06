import { RouteRecordRaw } from 'vue-router';
import { authGuard } from "@auth0/auth0-vue";

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '',
        component: () => import('pages/IndexPage.vue')
      },
      {
        path: 'people',
        component: () => import('pages/People.vue'),
        beforeEnter: authGuard
      },
    ]
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
