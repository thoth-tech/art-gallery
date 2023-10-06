<template>
  <div class="nav-site">
    <a-menu
      v-model:selectedKeys="current"
      mode="horizontal"
      class="nav-menu"
      disabledOverflow="{true}"
    >
      <a-sub-menu key="artworks">
        <template #title>
          <router-link to="/artworks" class="nav-link">
            Artworks
          </router-link>
        </template>
        <a-menu-item key="artwork-of-day" class="menu-item">
          <router-link to="/artworkofday" class="nav-sub">Artwork of the Day</router-link>
        </a-menu-item>
        <a-menu-item key="artworks-list">
          <router-link to="/artworks" class="nav-sub">Gallery of Artworks</router-link>
        </a-menu-item>
      </a-sub-menu>
      <a-sub-menu key="art-and-culture">
        <template #title>
          <router-link to="/culture" class="nav-link">Art & Culture</router-link>
        </template>
        <a-menu-item key="symbols">Symbols</a-menu-item>
        <a-menu-item key="artist-of-day">
          <router-link to="/artistofday" class="nav-sub">Artist of the Day</router-link>
        </a-menu-item>
        <a-menu-item key="art-facts">Art & Culture Facts</a-menu-item>
      </a-sub-menu>
      <a-sub-menu key="exhibitions">
        <template #title>
          <router-link to="/exhibition" class="nav-link">Exhibitions</router-link>
        </template>
        <a-menu-item key="exhibition-current">
          <router-link to="/exhibition" class="nav-sub">Current Exhibitions</router-link>
        </a-menu-item>
        <a-menu-item key="exhibition-past">Past Exhibitions</a-menu-item>
      </a-sub-menu>
      <a-sub-menu key="swagger" v-if="isAdmin()">
        <template #title>
          <a href="https://localhost:7194/swagger/index.html" class="nav-link">Swagger</a>
        </template>
      </a-sub-menu>
    </a-menu>
  </div>
</template>

<script>
import { Menu } from "ant-design-vue";
import { ref } from "vue";
import { mapState } from "vuex";

export default {
  name: "DropdownMenuAntD",
  components: {
    AMenu: Menu,
    ASubMenu: Menu.SubMenu,
    AMenuItem: Menu.Item,
  },
  methods: {
    isAdmin() {
      if (this.account.user) {
        return this.account.user.role == "Admin";
      }
    },
  },
  setup() {
    const current = ref(["home"]);
    return {
      current,
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
.nav-sub {
  font-size: 16px;
  padding-left: 0;
}
.nav-sub:hover {
  color: var(--color--turquoise-light-hover);
  transition: 0.1s;
  transition-delay: 0;
  border-bottom: 2px solid;
}

.nav-site > :deep(.ant-menu .ant-menu-submenu::after) {
  border-bottom: none;
}

.ant-menu-submenu::after {
  border-bottom: none;
}
</style>
