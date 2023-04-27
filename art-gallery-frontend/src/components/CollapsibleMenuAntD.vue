<template>
    <div>
        <a-menu v-model:openKeys="openKeys" v-model:selectedKeys="current" class="nav-menu-dropdown" mode="inline" @click="handleClick">
            <div>
            <a-sub-menu key="sub-artworks">
                <template #title>
                        <span>artworks</span>
                </template>
                <a-menu-item key="artwork-of-day">
                    <router-link to="/artworkofday" class="nav-sub">
                        <span>Artwork of the Day</span>
                    </router-link>
                </a-menu-item>
                <a-menu-item key="artworks-list">
                    <router-link to="/artworks" class="nav-sub">
                        <span>List of Artworks</span>
                    </router-link></a-menu-item>
            </a-sub-menu>
            <a-sub-menu key="sub-culture">
                <template #title>
                        <span>art & culture</span>
                </template>
                    <a-menu-item key="symbols"><span class="nav-sub">Symbols</span></a-menu-item>
                    <a-menu-item key="artist-of-day">
                        <router-link to="/artistofday" class="nav-sub">
                            <span>Artist of the Day</span>
                        </router-link>
                    </a-menu-item>
                    <a-menu-item key="art-facts"><span class="nav-sub">Aboriginal Art Facts</span></a-menu-item>
            </a-sub-menu>
            <a-sub-menu key="sub-exhibitions">
                <template #title>
                        <span>exhibitions</span>
                </template>
                <a-menu-item key="exhibition-current">
                    <router-link to="/exhibition" class="nav-sub">
                        <span>Current Exhibitions</span>
                    </router-link>
                </a-menu-item>
                <a-menu-item key="exhibition-past"><span class="nav-sub">Past Exhibitions</span></a-menu-item>
            </a-sub-menu>
        </div>
            <div>
            <a-menu-item key="login">
                <div class="nav-tools-login" v-if="!account.user">
                    <router-link to="/login" class="nav-link">
                        <span>Log In</span>
                    </router-link>
                </div>
            </a-menu-item>
            <a-menu-item key="signup">
                <div class="nav-tools-signup" v-if="!account.user">
                    <router-link to="/signup" class="nav-link">
                        <span>Sign Up</span>
                    </router-link>
                </div>
            </a-menu-item>
        </div>
        </a-menu>
    </div>
</template>


<script>
import { mapState } from 'vuex';
import { watch } from 'vue';

export default {
    name: "CollapsibleMenuAntD",
    setup() {
        const current = [];
        const openKeys = [];
        const handleClick = e => {
            console.log('click', e);
    };
    const titleClick = e => {
      console.log('titleClick', e);
    };
    watch(() => openKeys, val => {
      console.log('openKeys', val);
    });
    return {
      current,
      openKeys,
      handleClick,
      titleClick,
    };
  },
    computed: {
        ...mapState({
            account: state => state.account
        })
    }
}
</script>

<style scoped>
    .nav-sub:hover {
        color:var(--color--turquoise-light-hover);
        transition: .1s;
        transition-delay: 0;
        border-bottom: 2px solid;
    }
    .nav-menu-dropdown {
        display: grid;
        grid-template-columns: max-content auto;
    }
    .nav-sub {
        font-size: calc(12px + 2 * (100vw - 320px) / 1040);
    }
    .ant-menu::before {
        content: none;
    }
</style>