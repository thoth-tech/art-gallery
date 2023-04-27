<template>
  <div class="culture">
    <HeadingComponent title="ART & CULTURE"/>
    <HeadingComponent title="Artist of the Day" class="subheading"/>
    <FeaturedArtist  />
    <br>
    <br>
    <HeadingComponent title="Artwork of the Day" class="subheading"/>
    <FeaturedArtwork />
  </div>
</template>

<script>
import FeaturedArtist from '@/components/FeaturedArtist.vue';
import FeaturedArtwork from '@/components/FeaturedArtwork.vue';
import HeadingComponent from '@/components/HeadingComponent.vue';

export default {
  name: 'CultureView',
  components: {
    FeaturedArtist,
    HeadingComponent,
    FeaturedArtwork
  },
}
</script>

<style scoped>
  .subheading {
    font-style: italic;
    font-weight:normal;
    color: var(--color--gray);
  }
</style>