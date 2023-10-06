<template>
  <div>
    <a-menu :selectedKeys="current" class="nav-menu-dropdown" mode="inline" @click="handleClick">
      <a-sub-menu key="sub-artworks" title="Artworks">
        <a-menu-item key="artwork-of-day">
          <router-link to="/artworkofday" class="nav-sub"> Artwork of the Day </router-link>
        </a-menu-item>
        <a-menu-item key="artworks-list">
          <router-link to="/artworks" class="nav-sub"> List of Artworks </router-link>
        </a-menu-item>
      </a-sub-menu>
      <a-sub-menu key="sub-culture" title="Art & Culture">
        <a-menu-item key="symbols">Symbols</a-menu-item>
        <a-menu-item key="artist-of-day">
          <router-link to="/artistofday" class="nav-sub"> Artist of the Day </router-link>
        </a-menu-item>
        <a-menu-item key="art-facts">Aboriginal Art Facts</a-menu-item>
      </a-sub-menu>
      <a-sub-menu key="sub-exhibitions" title="Exhibitions">
        <a-menu-item key="exhibition-current">
          <router-link to="/exhibition" class="nav-sub"> Current Exhibitions </router-link>
        </a-menu-item>
        <a-menu-item key="exhibition-past">Past Exhibitions</a-menu-item>
      </a-sub-menu>
      <router-link v-if="!account.user" to="/login" class="nav-link">
        <a-menu-item key="login">Log In</a-menu-item>
      </router-link>
      <router-link v-if="!account.user" to="/signup" class="nav-link">
        <a-menu-item key="signup">Sign Up</a-menu-item>
      </router-link>
    </a-menu>
  </div>
</template>

<script>
import { Menu } from "ant-design-vue";
import { ref, watch } from "vue";
import { mapState } from "vuex";

export default {
  name: "CollapsibleMenuAntD",
  components: {
    AntMenu: Menu,
    AntSubMenu: Menu.SubMenu,
    AntMenuItem: Menu.Item,
  },
  setup() {
    const current = ref([]);
    const handleClick = (e) => {
      console.log("click", e);
    };

    watch(current, (val) => {
      console.log("current", val);
    });

    return {
      current,
      handleClick,
    };
  },
  computed: {
    ...mapState({
      account: (state) => state.account,
    }),
  },
};
</script>

<style scoped>
.nav-sub:hover {
  color: var(--color--turquoise-light-hover);
  transition: 0.1s;
  transition-delay: 0;
  border-bottom: 2px solid;
}
.nav-menu-dropdown {
  display: grid;
  grid-template-columns: max-content auto;
}
</style>
