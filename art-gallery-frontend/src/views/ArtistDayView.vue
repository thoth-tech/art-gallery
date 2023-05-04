<template>
    <div>
      <HeadingComponent title="ARTIST OF THE DAY"/>
      <br>
      <FeaturedArtist />
    </div>
</template>

<script>
import FeaturedArtist from '@/components/FeaturedArtist.vue';
import HeadingComponent from '@/components/HeadingComponent.vue';

export default {
  name: 'ArtistDayView',
  components: {
    HeadingComponent,
    FeaturedArtist
  }
}
</script>
