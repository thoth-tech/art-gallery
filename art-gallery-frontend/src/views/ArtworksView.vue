<template>
  <div class="artworks">
    <HeadingComponent title="ARTWORKS" />
    <GalleryArtworks />
  </div>
</template>

<script>
import HeadingComponent from "@/components/HeadingComponent.vue";
import GalleryArtworks from "../components/GalleryArtworks.vue";

export default {
  name: "ArtworksView",
  components: {
    GalleryArtworks,
    HeadingComponent,
  },
};
</script>

<style scoped>
.artworks {
  width: 70%;
  margin: auto;
}

.subheading {
  font-style: italic;
  font-weight: normal;
  color: var(--color--gray);
  font-size: max(25px);
}

@media only screen and (max-width: 600px) {
  .artworks {
    width: 95%;
  }
}
</style>
