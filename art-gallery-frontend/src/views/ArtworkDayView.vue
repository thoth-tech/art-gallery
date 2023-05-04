<template>
  <div>
    <HeadingComponent title="ARTWORK OF THE DAY"/>
    <br>
    <FeaturedArtwork />
  </div>
</template>

<script>
import FeaturedArtwork from '@/components/FeaturedArtwork.vue';
import HeadingComponent from '@/components/HeadingComponent.vue';

export default {
  name: 'ArtworkDayView',
  components: {
    HeadingComponent,
    FeaturedArtwork
  }
}
</script>
