<template>
    <nav class="navigation-component">
        <div class="navigation flex-spread">
            <div id="menus">
                <router-link to="/" class="nav-home">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512" class="nav-home-icon">
                        <!--! Font Awesome Pro 6.2.1 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2022 Fonticons, Inc. -->
                        <path d="M575.8 255.5c0 18-15 32.1-32 32.1h-32l.7 160.2c0 2.7-.2 5.4-.5 8.1V472c0 22.1-17.9 40-40 40H456c-1.1 0-2.2 0-3.3-.1c-1.4 .1-2.8 .1-4.2 .1H416 392c-22.1 0-40-17.9-40-40V448 384c0-17.7-14.3-32-32-32H256c-17.7 0-32 14.3-32 32v64 24c0 22.1-17.9 40-40 40H160 128.1c-1.5 0-3-.1-4.5-.2c-1.2 .1-2.4 .2-3.6 .2H104c-22.1 0-40-17.9-40-40V360c0-.9 0-1.9 .1-2.8V287.6H32c-18 0-32-14-32-32.1c0-9 3-17 10-24L266.4 8c7-7 15-8 22-8s15 2 21 7L564.8 231.5c8 7 12 15 11 24z"/>
                    </svg>
                </router-link>
                <div v-if="!this.showMenu && !this.usingAntDesign"><DropdownMenuHTML /></div>
                <div v-if="!this.showMenu && this.usingAntDesign"><DropdownMenuAntD /></div>
            </div>
            <div class="nav-tools">
                <div class="auth-tools" title="auth"  v-if="!this.showMenu">
                    <div class="user-name" title="user-name" v-if="account.user">
                        <span>Signed in as <b>{{ account.user.name }}</b></span>
                    </div>
                    <div @click="logout" class="nav-tools-login" title="Logout" v-if="account.user">
                        <span class="login-nav-button">logout</span>
                    </div>
                    <div @click="toggleLogin" class="nav-tools-login" title="Login" v-if="(!account.user && $route.name !== 'login')">
                        <span class="logout-nav-button">login</span>
                    </div>
                </div>
                <div @click="toggleMenu" class="nav-tools-menu" title="Menu">
                    <span class="screen-reader-only">Menu</span>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                        <!--! Font Awesome Pro 6.2.1 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2022 Fonticons, Inc. -->
                        <path d="M0 96C0 78.3 14.3 64 32 64H416c17.7 0 32 14.3 32 32s-14.3 32-32 32H32C14.3 128 0 113.7 0 96zM0 256c0-17.7 14.3-32 32-32H416c17.7 0 32 14.3 32 32s-14.3 32-32 32H32c-17.7 0-32-14.3-32-32zM448 416c0 17.7-14.3 32-32 32H32c-17.7 0-32-14.3-32-32s14.3-32 32-32H416c17.7 0 32 14.3 32 32z"/>
                    </svg>
                </div>
            </div>
        </div>
        <div class="menu-grid" v-if="this.showMenu && !this.usingAntDesign"><CollapsibleMenuHTML /></div>
        <div class="menu-grid" v-if="this.showMenu && this.usingAntDesign"><CollapsibleMenuAntD /></div>

        <div class="login-box" v-if="(this.showLogin && !account.user && $route.name !== 'login')">
            <LoginComponent />
        </div>
    </nav>
</template>

<script>
import LoginComponent from '@/components/LoginComponent.vue';
import { mapState } from 'vuex';
import { userService } from '@/services/UserService';
import DropdownMenuAntD from '@/components/DropdownMenuAntD.vue';
import DropdownMenuHTML from '@/components/DropdownMenuHTML.vue';
import CollapsibleMenuHTML from '@/components/CollapsibleMenuHTML.vue';
import CollapsibleMenuAntD from '@/components/CollapsibleMenuAntD.vue';

export default {
    data () {
        return {
            usingAntDesign: true,
            showMenu: false,
            showLogin: false
        }
    },
    watch: {
            // eslint-disable-next-line
            $route (to, from){
                this.showLogin = false;
                this.showMenu = false;
            }
        },
    components: {
        LoginComponent,
        DropdownMenuAntD,
        DropdownMenuHTML,
        CollapsibleMenuHTML,
        CollapsibleMenuAntD
    },
    methods: {
        toggleLogin () {
            if (this.showLogin === false) {
                this.showMenu = false
                this.showLogin = true
            }
            else {
                this.showLogin = false
            }
        },
        toggleMenu () {
            if (this.showMenu === false) {
                this.showLogin = false
                this.showMenu = true
            }
            else {
                this.showMenu = false
            }
        },
        logout () {
            userService.Logout()
        }
    },
    computed: {
        ...mapState({
            account: state => state.account
        })
    }
}
</script>

<style scoped>
    .navigation-component {
        border-bottom: 1px solid var(--color--grey-med-light);
    }

    .nav-tools, .auth-tools, :deep(.nav-site), :deep(.nav-menu) {
        cursor:pointer;
        display: inline-flex;
        position: relative;
    }

    .login-nav-button, .logout-nav-button {
        color: var(--color--primary);
        cursor:pointer;
        text-transform:uppercase;
        font-weight: var(--font--semibold);
    }

    .nav-home-icon, .nav-tools-menu svg{
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

    :deep(.nav-link:hover), .login-nav-button:hover, .logout-nav-button:hover {
        color:var(--color--turquoise-light-hover);
        transition: .1s;
        transition-delay: 0;
        border-bottom: 3px solid;
    }

    :deep(.nav-menu), :deep(.nav-menu-dropdown) {
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

    :deep(.nav-tools-signup), :deep(.nav-tools-login) {
        margin-left: 0;
        margin-right: calc(2px + 10 * (100vw - 320px) / 1040);
        text-align: right;
    }

    .nav-tools {
        text-align: right;
        margin-right: calc(2px + 10 * (100vw - 320px) / 1040);
    }

    .nav-tools-menu svg{
        fill: var(--color--black);
    }

    svg:hover, .nav-tools svg:hover {
        transition: .1s;
        transition-delay: 0;
        fill:var(--color--turquoise-light);
    }

    .login-box {
        position: absolute;
        text-align: right;
    }

    @media only screen and (max-width: 600px) {
        :deep(.nav-menu){
            display: none;
        }

        :deep(.nav-tools-signup a) {
            color: var(--color--black);
        }

        .login-box {
        position:inherit;
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
