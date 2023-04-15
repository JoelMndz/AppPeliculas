import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/registro',
      name: 'registro',
      component: () => import('../views/RegistroView.vue')
    },
    {
      path: '/peliculas',
      name: 'peliculas',
      component: () => import('../views/PeliculaView.vue')
    },
    {
      path: '/generos',
      name: 'generos',
      component: () => import('../views/GeneroView.vue')
    },
  ]
})

export default router
