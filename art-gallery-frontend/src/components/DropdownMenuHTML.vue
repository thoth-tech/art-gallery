<template>
    <div class="nav-site">
        <ul class="nav-menu">
            <li>
                <router-link to="/" class="nav-link">
                    <span>Home</span>
                </router-link>
            </li>
            <li>
                <router-link to="/artworks" class="nav-link subnav">
                    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
                    <span>Artworks <i class="fa fa-caret-down"></i></span>
                    <div class="subnav-content">
                        <router-link to="/artworkofday" class="nav-sub">
                        <span>Artwork of the Day</span>
                        </router-link>
                        <br>
                        <br>
                        <router-link to="/artworks" class="nav-sub">
                        <span>List of Artworks</span>
                        </router-link>
                    </div>
                </router-link>
            </li>
            <li>
                <router-link to="/culture" class="nav-link subnav">
                    <span>Art & Culture <i class="fa fa-caret-down"></i></span>
                    <div class="subnav-content">
                        <span class="nav-sub">Symbols</span>
                        <br>
                        <br>
                        <router-link to="/artistofday" class="nav-sub">
                            <span>Artist of the Day</span>
                        </router-link>
                        <br>
                        <br>
                        <span class="nav-sub">Aboriginal Art Facts</span>
                    </div>
                </router-link>
            </li>
            <li>
                <router-link to="/exhibition" class="nav-link">
                    <span>Exhibitions</span>
                </router-link>
            </li>
        </ul>
        <div v-if="isAdmin()">
            <ul class="nav-menu">
                <li>
                    <a href="https://localhost:7194/swagger/index.html" class="nav-link">Swagger</a>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
import { mapState } from 'vuex';

export default {
    name: "DropdownMenuHTML",
    methods: {
        isAdmin() {
            if (this.account.user) {
                return this.account.user.role == "Admin";
            }
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
    .subnav {
        overflow: hidden;
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
        display:block;
        margin-left: 15px;
        padding: 10px;
        color: black;
    }

    .nav-sub {
        font-size: 16px;
        padding-left: 0;
    }
    .nav-sub:hover {
        color:var(--color--turquoise-light-hover);
        transition: .1s;
        transition-delay: 0;
        border-bottom: 2px solid;
    }

</style>