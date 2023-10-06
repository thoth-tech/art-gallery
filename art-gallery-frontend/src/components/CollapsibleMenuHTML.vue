<template>
  <div>
    <ul class="nav-menu-dropdown">
      <li>
        <router-link to="/artworks" class="nav-link nav-link-1">
          <span>Artworks</span>
        </router-link>
        <router-link to="/artworkofday" class="nav-sub nav-link-1">
          <span>Artwork of the Day</span>
        </router-link>
      </li>
      <li>
        <router-link to="/culture" class="nav-link nav-link-2">
          <span>Art & Culture</span>
        </router-link>
        <router-link to="/artistofday" class="nav-sub nav-link-2">
          <span>Artist of the Day</span>
        </router-link>
      </li>
      <li v-if="!account.user" class="nav-link nav-link-3">
        <router-link to="/login">
          <span>Log In</span>
        </router-link>
        <router-link to="/signup">
          <span>Sign Up</span>
        </router-link>
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
import { mapState } from "vuex";

export default {
  name: "CollapsibleMenuHTML",
  computed: {
    ...mapState({
      account: (state) => state.account,
    }),
  },
};
</script>

<style scoped>
/* Add classes to style individual elements */
.nav-menu-dropdown {
  display: grid;
  grid-template-columns: auto auto 100px;
}

.nav-link {
  font-weight: lighter;
  font-size: smaller;
  padding-left: 40px;
}

.nav-link-1 {
  grid-column-start: 1;
}

.nav-link-2 {
  grid-column-start: 2;
}

.nav-link-3 {
  grid-column-start: 3;
  margin-left: 0;
}

.nav-sub {
  font-weight: lighter;
  font-size: smaller;
  padding-left: 40px;
}
</style>
