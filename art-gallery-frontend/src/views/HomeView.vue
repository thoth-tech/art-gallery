<template>
  <div class="home">
    <div class="user-greeting" v-if="account.user">
      <span>Hi {{ account.user.name }}!</span>
    </div>
<<<<<<< HEAD
    <div class="home-banner">
      <picture>
        <source srcset="@/assets/img/the_dance_class_2014.79.710.jpg" media="(min-width: 1200px)">
        <source srcset="@/assets/img/the_dance_class_2014.79.710_medium.webp" media="(min-width: 768px)">
        <img src="@/assets/img/the_dance_class_2014.79.710_small.webp" alt="Front Splash Image">
      </picture>
    </div>
=======
    <HeadingComponent title="The Art Gallery Project" />
    <HeadingComponent title="Artist of the Day" class="subheading" />
    <FeaturedArtist />
    <br />
    <br />
    <HeadingComponent title="Artwork of the Day" class="subheading" />
    <FeaturedArtwork />
>>>>>>> b5973031480f264c7563a32b1c171c6c35f23eec
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
