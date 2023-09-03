<template>
  <nav class="navigation-component">
    <div class="navigation flex-spread">
      <div id="menus">
        <router-link to="/" class="nav-home">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512" class="nav-home-icon">
            <!-- ... SVG content for home icon ... -->
          </svg>
        </router-link>
        <div v-if="!showMenu && !usingAntDesign"><DropdownMenuHTML /></div>
        <div v-if="!showMenu && usingAntDesign"><DropdownMenuAntD /></div>
      </div>
      <div class="nav-tools">
        <div class="auth-tools" title="auth" v-if="!showMenu">
          <div class="user-name" title="user-name" v-if="account.user">
            <span>Signed in as <b>{{ account.user.name }}</b></span>
          </div>
          <div @click="logout" class="nav-tools-login" title="Logout" v-if="account.user">
            <span class="login-nav-button">logout</span>
          </div>
          <div
            @click="toggleLogin"
            class="nav-tools-login"
            title="Login"
            v-if="!account.user && $route.name !== 'login'"
          >
            <span class="logout-nav-button">login</span>
          </div>
        </div>
        <div @click="toggleMenu" class="nav-tools-menu" title="Menu">
          <span class="screen-reader-only">Menu</span>
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
            <!-- ... SVG content for menu icon ... -->
          </svg>
        </div>
      </div>
    </div>
    <div class="menu-grid" v-if="showMenu && !usingAntDesign">
      <CollapsibleMenuHTML />
    </div>
    <div class="menu-grid" v-if="showMenu && usingAntDesign"><CollapsibleMenuAntD /></div>

    <div class="login-box" v-if="showLogin && !account.user && $route.name !== 'login'">
      <LoginComponent />
    </div>
  </nav>
</template>

<script>
import LoginComponent from "@/components/LoginComponent.vue";
import { mapState } from "vuex";
import { userService } from "@/services/UserService";
import DropdownMenuAntD from "@/components/DropdownMenuAntD.vue";
import DropdownMenuHTML from "@/components/DropdownMenuHTML.vue";
import CollapsibleMenuHTML from "@/components/CollapsibleMenuHTML.vue";
import CollapsibleMenuAntD from "@/components/CollapsibleMenuAntD.vue";

export default {
  data() {
    return {
      usingAntDesign: true,
      showMenu: false,
      showLogin: false,
    };
  },
  watch: {
    // eslint-disable-next-line
    $route(to, from) {
      this.showLogin = false;
      this.showMenu = false;
    },
  },
  components: {
    LoginComponent,
    DropdownMenuAntD,
    DropdownMenuHTML,
    CollapsibleMenuHTML,
    CollapsibleMenuAntD,
  },
  methods: {
    toggleLogin() {
      if (this.showLogin === false) {
        this.showMenu = false;
        this.showLogin = true;
      } else {
        this.showLogin = false;
      }
    },
    toggleMenu() {
      if (this.showMenu === false) {
        this.showLogin = false;
        this.showMenu = true;
      } else {
        this.showMenu = false;
      }
    },
    logout() {
      userService.Logout();
    },
  },
  computed: {
    ...mapState({
      account: (state) => state.account,
    }),
  },
};
</script>

<style scoped>
.navigation-component {
  border-bottom: 1px solid var(--color--grey-med-light);
}

.nav-tools,
.auth-tools,
:deep(.nav-site),
:deep(.nav-menu) {
  cursor: pointer;
  display: inline-flex;
  position: relative;
}

.login-nav-button,
.logout-nav-button {
  color: var(--color--primary);
  cursor: pointer;
  text-transform: uppercase;
  font-weight: var(--font--semibold);
}

.nav-home-icon,
.nav-tools-menu svg {
  max-width: 28px;
  width: calc(24px + 6 * (100vw - 320px) / 1040);
  margin-left: calc(10px + 2 * (100vw - 320px) / 1040);
  margin-right: 5px;
}

.nav-home-icon {
  margin-top: 12px;
}

.nav-tools {
  margin-top: 10px;
}

#menus {
  display: inline-flex;
}

:deep(.nav-link.router-link-exact-active) {
  color: var(--color--turquoise-light);
  border-bottom: 2px solid;
}

:deep(.nav-sub.router-link-exact-active) {
  color: var(--color--turquoise-light);
}

:deep(.nav-link:hover),
.login-nav-button:hover,
.logout-nav-button:hover {
  color: var(--color--turquoise-light-hover);
  transition: 0.1s;
  transition-delay: 0;
  border-bottom: 3px solid;
}

:deep(.nav-menu),
:deep(.nav-menu-dropdown) {
  font-family: var(--font--base);
  font-weight: var(--font--normal);
  padding: 0;
  list-style: none;
}

:deep(.nav-menu) {
  font-size: calc(16px + 3 * (100vw - 580px) / 1040);
}

.auth-tools {
  margin-top: 4px;
  font-size: calc(12px + 3 * (100vw - 320px) / 1040);
}

.auth-tools .user-name {
  margin-right: 5px;
}

:deep(.nav-menu-dropdown) {
  font-size: calc(18px + 4 * (100vw - 320px) / 1040);
}

:deep(.nav-link) {
  margin-left: calc(10px + 10 * (100vw - 320px) / 1040);
  color: var(--color--black);
  text-transform: lowercase;
}

:deep(.nav-tools-signup),
:deep(.nav-tools-login) {
  margin-left: 0;
  margin-right: calc(2px + 10 * (100vw - 320px) / 1040);
  text-align: right;
}

.nav-tools {
  text-align: right;
  margin-right: calc(2px + 10 * (100vw - 320px) / 1040);
}

.nav-tools-menu svg {
  fill: var(--color--black);
}

svg:hover,
.nav-tools svg:hover {
  transition: 0.1s;
  transition-delay: 0;
  fill: var(--color--turquoise-light);
}

.login-box {
  position: absolute;
  text-align: right;
}

@media only screen and (max-width: 600px) {
  :deep(.nav-menu) {
    display: none;
  }

  :deep(.nav-tools-signup a) {
    color: var(--color--black);
  }

  .login-box {
    position: inherit;
  }

  :deep(.container) {
    margin: none;
    text-align: right;
    max-width: 600px;
  }

  :deep(.card) {
    width: 95%;
    max-width: 600px;
  }
}
</style>
