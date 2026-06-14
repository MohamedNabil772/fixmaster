import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/auth',
      component: () => import('../layouts/AuthLayout.vue'),
      children: [
        {
          path: '/login',
          name: 'login',
          component: () => import('../views/Login.vue')
        },
        {
          path: '/register',
          name: 'register',
          component: () => import('../views/Register.vue')
        }
      ]
    },
    {
      path: '/admin',
      component: () => import('../layouts/AdminLayout.vue'),
      meta: { requiresAuth: true, roles: ['Admin'] },
      children: [
        {
          path: '',
          redirect: '/admin/dashboard'
        },
        {
          path: 'dashboard',
          name: 'admin-dashboard',
          component: () => import('../views/Dashboard.vue')
        },
        {
          path: 'services',
          name: 'service-management',
          component: () => import('../views/admin/ServiceManagement.vue')
        }
      ]
    },
    {
      path: '/unauthorized',
      name: 'unauthorized',
      component: () => import('../views/UnauthorizedPage.vue')
    },
    {
      path: '/',
      component: () => import('../layouts/AppLayout.vue'),
      meta: { requiresAuth: true },
      children: [
        {
          path: '',
          redirect: '/dashboard'
        },
        {
          path: 'dashboard',
          name: 'dashboard',
          component: () => import('../views/Dashboard.vue')
        },
        {
          path: 'requests/:id',
          name: 'request-detail',
          component: () => import('../views/RequestDetail.vue')
        }
      ]
    }
  ]
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login')
  } else if ((to.path === '/login' || to.path === '/register') && authStore.isAuthenticated) {
    next('/dashboard')
  } else if (to.meta.roles && !to.meta.roles.includes(authStore.user?.role)) {
    next('/unauthorized')
  } else {
    next()
  }
})

export default router
