<template>
  <div>
    <div v-if="isArtistOfDayLoaded">
      <LazyLoadingCardComponent :subheading="artistOfDay.displayName" :imageURL="artistOfDay.profileImageUrl" :detail1="birthInfo" />
    </div>
    <div v-else>Loading artist of the day...</div>
  </div>
</template>

<script>
import CardComponent from "./CardComponent.vue";
import { getArtistOfTheDay } from "../services/ArtistService";
import LazyLoadingCardComponent from "./LazyLoadingCardComponent.vue";

export default {
  name: "FeaturedArtist",
  components: { CardComponent, LazyLoadingCardComponent },
  data() {
    return {
      artistOfDay: null,
      isArtistOfDayLoaded: false,
    };
  },
  computed: {
    birthInfo() {
      if (this.artistOfDay) {
        return `Born: ${this.artistOfDay.yearOfBirth}, ${this.artistOfDay.placeOfBirth}`;
      }
      return "";
    },
  },
  async mounted() {
    try {
      this.artistOfDay = await getArtistOfTheDay();
      this.isArtistOfDayLoaded = true;
    } catch (error) {
      console.error("Error fetching artist of the day:", error);
    }
  },
};
</script>
