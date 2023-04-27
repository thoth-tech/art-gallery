<template>
    <div>
        <ul class="nav-menu-dropdown">
            <li>
                <router-link to="/artworks" class="nav-link nav-link-1">
                    <span>Artworks</span>
                </router-link>
            </li>
            <li>
                <router-link to="/culture" class="nav-link nav-link-2">
                    <span>Art & Culture</span>
                </router-link>
            </li>
            <li>
                <div class="nav-tools-login" v-if="!account.user">
                    <router-link to="/login" class="nav-link nav-link-3">
                        <span>Log In</span>
                    </router-link>
                </div>
            </li>
            <li>
                    <router-link to="/artworkofday" class="nav-sub nav-link-1">
                    <span>Artwork of the Day</span>
                    </router-link>
            </li>
            <li>
                <router-link to="/artistofday" class="nav-sub nav-link-2">
                    <span>Artist of the Day</span>
                </router-link>
            </li>
            <li>
                <div class="nav-tools-signup" v-if="!account.user">
                    <router-link to="/signup" class="nav-link nav-link-3">
                        <span>Sign Up</span>
                    </router-link>
                </div>
            </li>
            <li>
                <router-link to="/exhibition" class="nav-link nav-link-1">
                    <span>Exhibitions</span>
                </router-link>
            </li>
        </ul>
    </div>
</template>
<script>
import { mapState } from 'vuex';

export default {
    name: "CollapsibleMenuHTML",
    computed: {
        ...mapState({
            account: state => state.account
        })
    }
}
</script>

<style scoped>
    .nav-menu-dropdown {
        display: grid;
        grid-template-columns: auto auto 100px;
    }

    :deep(.nav-link-1) {
        grid-column-start: 1;
    }

    :deep(.nav-link-2) {
        grid-column-start: 2;
    }

    :deep(.nav-link-3) {
        grid-column-start: 3;
        margin-left: 0;
    }

    :deep(.nav-sub) {
        font-weight: lighter;
        font-size: smaller;
        padding-left: 40px;
    }
</style>