<template>
    <div class="nav-site">
      <ul class="nav-menu">
        <li>
          <router-link to="/" class="nav-link">Home</router-link>
        </li>
        <li class="subnav">
          <router-link to="/artworks" class="nav-link">
            <i class="fa fa-caret-down"></i>
            Artworks
          </router-link>
          <ul class="subnav-content">
            <li>
              <router-link to="/artworkofday" class="nav-sub">Artwork of the Day</router-link>
            </li>
            <br />
            <br />
            <li>
              <router-link to="/artworks" class="nav-sub">List of Artworks</router-link>
            </li>
          </ul>
        </li>
        <li class="subnav">
          <router-link to="/culture" class="nav-link">
            <i class="fa fa-caret-down"></i>
            Art & Culture
          </router-link>
          <ul class="subnav-content">
            <li>
              <span class="nav-sub">Symbols</span>
            </li>
            <br />
            <br />
            <li>
              <router-link to="/artistofday" class="nav-sub">Artist of the Day</router-link>
            </li>
            <br />
            <br />
            <li>
              <span class="nav-sub">Aboriginal Art Facts</span>
            </li>
          </ul>
        </li>
        <li>
          <router-link to="/exhibition" class="nav-link">Exhibitions</router-link>
        </li>
      </ul>
      <ul class="nav-menu" v-if="isAdmin()">
        <li>
          <a href="https://localhost:7194/swagger/index.html" class="nav-link">Swagger</a>
        </li>
      </ul>
    </div>
  </template>
  
  <script>
  import { mapState } from "vuex";
  
  export default {
    name: "DropdownMenu",
    computed: {
      ...mapState({
        account: (state) => state.account,
      }),
    },
    methods: {
      isAdmin() {
        return this.account?.user?.role === "Admin";
      },
    },
  };
  </script>
  
  <style scoped>
  .subnav {
    position: relative;
  }
  
  .subnav-content {
    display: none;
    position: absolute;
    z-index: 1;
    background-color: white;
    font-size: smaller;
    box-shadow: 0px 0px 5px 1px rgba(0, 0, 0, 0.205);
  }
  
  .subnav:hover .subnav-content {
    display: block;
    margin-left: 15px;
    padding: 10px;
    color: black;
  }
  
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
  </style>
  