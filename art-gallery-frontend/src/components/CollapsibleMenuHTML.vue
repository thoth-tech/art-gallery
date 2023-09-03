<template>
  <div>
    <ul class="nav-menu-dropdown">
      <!-- Artworks and Art & Culture links -->
      <li>
        <router-link to="/artworks" class="nav-link">
          <span>Artworks</span>
        </router-link>
      </li>
      <li>
        <router-link to="/culture" class="nav-link">
          <span>Art & Culture</span>
        </router-link>
      </li>

      <!-- Conditional login/signup links -->
      <li v-if="!account.user">
        <router-link to="/login" class="nav-link">
          <span>Log In</span>
        </router-link>
      </li>
      <li v-if="!account.user">
        <router-link to="/signup" class="nav-link">
          <span>Sign Up</span>
        </router-link>
      </li>

      <!-- Artwork of the Day and Artist of the Day links -->
      <li>
        <router-link to="/artworkofday" class="nav-link nav-sub">
          <span>Artwork of the Day</span>
        </router-link>
      </li>
      <li>
        <router-link to="/artistofday" class="nav-link nav-sub">
          <span>Artist of the Day</span>
        </router-link>
      </li>

      <!-- Exhibitions link -->
      <li>
        <router-link to="/exhibition" class="nav-link">
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
.nav-menu-dropdown {
  display: flex;
  justify-content: space-between;
  list-style: none;
  padding: 0;
}

.nav-link {
  padding: 10px;
  text-align: center;
  text-decoration: none;
  color: black;
}

.nav-link.nav-sub {
  font-weight: lighter;
  font-size: smaller;
  padding-left: 40px;
}
</style>
