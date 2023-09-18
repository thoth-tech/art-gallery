<template>
  <div class="home">
    <div class="user-greeting" v-if="account.user">
      <span>Hi {{ account.user.name }}!</span>
    </div>
    <HeadingComponent title="The Art Gallery Project" />
    <HeadingComponent title="Artist of the Day" class="subheading" />
    <FeaturedArtist />
    <br />
    <br />
    <HeadingComponent title="Artwork of the Day" class="subheading" />
    <FeaturedArtwork />
  </div>
</template>

<script>
import HeadingComponent from "@/components/HeadingComponent.vue";
import FeaturedArtist from "@/components/FeaturedArtist.vue";
import FeaturedArtwork from "@/components/FeaturedArtwork.vue";
import { mapState } from "vuex";

export default {
  name: "HomeView",
  components: {
    HeadingComponent,
    FeaturedArtist,
    FeaturedArtwork,
  },
  computed: {
    ...mapState({
      account: (state) => state.account,
    }),
  },
};
</script>

<style scoped>
.home-banner {
  text-align: center;
}

.home img {
  width: 90vw;
  max-width: 1000px;
}

.home {
  font-family: var(--font--base);
  margin: 20px;
  padding-bottom: 50px;
}

.user-greeting {
  font-family: var(--font--base);
  margin: 15px;
  text-align: center;
  font-size: 2em;
  font-weight: 600;
}

.subheading {
  font-style: italic;
  font-weight: normal;
  color: var(--color--gray);
}
</style>
