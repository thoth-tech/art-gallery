import { createRouter, createWebHistory } from 'vue-router'
import { userService } from '@/services/UserService';
import HomeView from '@/views/HomeView.vue'
import ArtworksView from '@/views/ArtworksView'
import CultureView from '@/views/CultureView'
import ExhibitionsView from '@/views/ExhibitionsView'
import SignUpView from '@/views/SignUpView'
import AboutView from '@/views/AboutView'
import ContactView from '@/views/ContactView'
import LoginView from '@/views/LoginView'
import ArtistDayView from '@/views/ArtistDayView'
import ArtworkDayView from '@/views/ArtworkDayView'
import { Date } from 'core-js'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/artworks',
    name: 'artworks',
    component: ArtworksView
  },
  {
    path: '/culture',
    name: 'culture',
    component: CultureView
  },
  {
    path: '/exhibition',
    name: 'exhibition',
    component: ExhibitionsView
  },
  {
    path: '/signup',
    name: 'signup',
    component: SignUpView
  },
  {
    path: '/about',
    name: 'about',
    component: AboutView
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView
  },
  {
    path: '/artistofday',
    name: 'artistofday',
    component: ArtistDayView
  },
  {
    path: '/artworkofday',
    name: 'artworkofday',
    component: ArtworkDayView
  },
  {
    path: '/contact',
    name: 'contact',
    component: ContactView
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  // Check if there is a user logged in
  if (JSON.parse(localStorage.getItem('user')) != null) {
    // If token is expired log out
    const expired = JSON.parse(localStorage.getItem('user')).expiry;
    if (expired * 1000 - Date.now() < 0) {
        userService.Logout();
    }
  }
  next();
});

export default router
