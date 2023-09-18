<template>
  <div>
    <div class="gallery">
      <div class="gallery-panel" v-for="artwork in artworks" :key="artwork.id" >
        <img :src="artwork.primaryImageUrl" alt="Artwork" @click="openLightbox(artwork)"/>
      </div>
    </div>

    <!-- Lightbox -->
    <div class="lightbox" v-if="lightbox">
      <div class="lightbox-content">
        <img :src="selectedArtwork.primaryImageUrl" alt="Artwork" />
        <div class="image-details">
          <p><span class="lightbox-header" >Title:</span> {{ selectedArtwork.title }}</p>
          <p><span class="lightbox-header" >Media:</span> {{ selectedArtwork.mediaType }}</p>
          <p><span class="lightbox-header" >Year:</span> {{ selectedArtwork.yearCreated }}</p>
          <p><span class="lightbox-header" >Description:</span> {{ selectedArtwork.description }}</p>
          <p><span class="lightbox-header" >Price:</span> ${{ selectedArtwork.price.toFixed(2) }}</p>
          <p><span class="lightbox-header" >Contributing Artist&#40;s&#41;:</span> {{ selectedArtwork.contributingArtists }}</p>
        </div>
        <button class="close-lightbox" @click="closeLightbox()">Close</button>
      </div>
    </div>
  </div>
</template>

<script>
import { getArtworks } from "../services/ArtworkService";

export default {
  name: "GalleryArtworks",
  data() {
    return {
      artworks: [],
      contributingArtists: "",
      lightbox: false,
      selectedArtwork: null,
    };
  },
  methods: {
    async fetchArtworks() {
      await getArtworks().then((data) => {
        this.artworks = data;
      });
      this.getContributingArtists();
    },
    getContributingArtists() {
      this.artworks.forEach((item) => {
        item.contributingArtists = item.contributingArtists.join(", ");
      });
    },
    openLightbox(artwork) {
      // console.log("lightbox open")
      this.selectedArtwork = artwork;
      this.lightbox = true;
    },
    closeLightbox() {
      // console.log("lightbox closed")
      this.lightbox = false;
      this.selectedArtwork = null;
    },
  },
  mounted() {
    this.fetchArtworks();
  },
};
</script>

<style>
.gallery {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  grid-template-rows: repeat(auto-fill, minmax(200px, 1fr));
  grid-gap: 2rem;
  max-width: 2500px;
  margin: 0 auto;
  padding: 0 1rem;
  width: 100%;
  overflow-y: scroll;
  margin-top: 100px;
  margin-bottom: 20px;
}
/*Gallery pannels when image clicked */
.gallery-panel img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 0.75rem;
}

/* Lightbox styles */
.lightbox {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.8);
  text-align: center;
  z-index: 10;
}

.lightbox-content {
  position: absolute;
  width: 50%;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: white;
  padding: 20px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
}

.lightbox img {
  max-width: 100%;
}

.lightbox .image-details {
  margin-top: 20px;
  margin-bottom: 10px;
}

.lightbox .image-details p {
  margin: auto;
  padding: 2px;
  font-size: 16px;
}

.lightbox .lightbox-header {
  font-weight: bold;
}

.lightbox button {
  box-sizing: border-box;
  border-radius: 60px 60px 60px 60px;
  background-color: #0b7161;
  border: 1px solid #0b7161;
  color: #FFF;
  font-size: 12px;
  letter-spacing: 0.35px;
  text-transform: uppercase;
  padding: 1px;
  padding-left: 6px;
  padding-right: 6px;
}

.lightbox button:hover {
  color: #00818a;
  border: 1px solid #0b7161;
  background-color: #FFF;
  cursor: pointer;
}

@media only screen and (max-width: 1000px) {
  .lightbox-content {
    width: 80%;
  }
}

@media only screen and (max-width: 4000px) and (min-width: 2000px) {
  .lightbox .image-details p {
    font-size: 40px;
  }

  .lightbox button {
    border: 3px solid #0b7161;
    font-size: 22px;
    padding: 5px;
    padding-left: 15px;
    padding-right: 15px;
  }

  .lightbox button:hover {
    color: #00818a;
    border: 3px solid #0b7161;
    background-color: #FFF;
    cursor: pointer;
  }
}
</style>
