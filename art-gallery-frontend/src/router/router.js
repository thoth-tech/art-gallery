import { userService } from "@/services/UserService";
import { createRouter, createWebHistory } from "vue-router";

// implementing lazy loading (chunks)
const HomeView = () => import(/* webpackChunkName: "home" */ "@/views/HomeView.vue");
const ArtworksView = () => import(/* webpackChunkName: "artworks" */ "@/views/ArtworksView");
const CultureView = () => import(/* webpackChunkName: "culture" */ "@/views/CultureView");
const ExhibitionsView = () => import(/* webpackChunkName: "exhibitions" */ "@/views/ExhibitionsView");
const SignUpView = () => import(/* webpackChunkName: "signup" */ "@/views/SignUpView");
const AboutView = () => import(/* webpackChunkName: "about" */ "@/views/AboutView");
const ContactView = () => import(/* webpackChunkName: "contact" */ "@/views/ContactView");
const LoginView = () => import(/* webpackChunkName: "login" */ "@/views/LoginView");
const ArtistDayView = () => import(/* webpackChunkName: "artistofday" */ "@/views/ArtistDayView");
const ArtworkDayView = () => import(/* webpackChunkName: "artworkofday" */ "@/views/ArtworkDayView");

import { Date } from "core-js";

const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/artworks",
    name: "artworks",
    component: ArtworksView,
  },
  {
    path: "/culture",
    name: "culture",
    component: CultureView,
  },
  {
    path: "/exhibition",
    name: "exhibition",
    component: ExhibitionsView,
  },
  {
    path: "/signup",
    name: "signup",
    component: SignUpView,
  },
  {
    path: "/about",
    name: "about",
    component: AboutView,
  },
  {
    path: "/login",
    name: "login",
    component: LoginView,
  },
  {
    path: "/artistofday",
    name: "artistofday",
    component: ArtistDayView,
  },
  {
    path: "/artworkofday",
    name: "artworkofday",
    component: ArtworkDayView,
  },
  {
    path: "/contact",
    name: "contact",
    component: ContactView,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  // Check if there is a user logged in
  if (JSON.parse(localStorage.getItem("user")) != null) {
    // If token is expired log out
    const expired = JSON.parse(localStorage.getItem("user")).expiry;
    if (expired * 1000 - Date.now() < 0) {
      userService.Logout();
    }
  }
  next();
});

export default router;
