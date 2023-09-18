<template v-if="dataLoaded">
  <CardComponent
  v-for="artwork in artworks"
    :subheading="artworkOfDay.title"
    :imageURL="artworkOfDay.primaryImageUrl"
    :detail1="artworkOfDay.mediaType"
    :detail2="String(artworkOfDay.yearCreated)"
    :detail3="contributingArtists"
    :detail4="artworkOfDay.description"
    :detail5="`, $` + artworkOfDay.price"
  />
</template>

<script>
import CardComponent from "./CardComponent.vue";
import { getArtworkOfTheDay } from "../services/ArtworkService";

export default {
  name: "FeaturedArtwork",
  components: { CardComponent },
  data() {
    return {
      artworkOfDay: [],
      dataLoaded: false,
      contributingArtists: "",
    };
  },
  methods: {
    // Gets the data from endpoint and stores in an array.
    async fetchArtworkOfTheDay() {
      await getArtworkOfTheDay().then((data) => {
        this.artworkOfDay = data;
        this.artworkOfDay.price = this.artworkOfDay.price.toFixed(2)
      });
      this.getContributingArtists();
      this.dataLoaded = true;
    },

    getContributingArtists() {
      this.contributingArtists = this.artworkOfDay.contributingArtists.join(", ");
    },
  },
  mounted() {
    this.fetchArtworkOfTheDay();
  },
};
</script>

<style>
.card-component {
  width: 300px;
  margin: 10px;
  border: 1px solid #ccc;
  padding: 10px;
}
</style>