<template v-if="dataLoaded">
    <CardComponent
        :subheading="artistOfDay.displayName"
        :imageURL="artistOfDay.profileImageUrl"
        :detail1="(`Born: ` + artistOfDay.yearOfBirth + `, ` + artistOfDay.placeOfBirth)"/>
</template>

<script>
import CardComponent from './CardComponent.vue';
import { getArtistOfTheDay } from '../services/ArtistService'

export default {
    name: "FeaturedArtist",
    components: { CardComponent },
    data() {
        return {
            artistOfDay: [],
            dataLoaded: false,
        }
    },
    methods: {
        // Gets the data from endpoint and stores in an array.
        async fetchArtistOfTheDay() {
            await getArtistOfTheDay()
                .then(data => {
                    this.artistOfDay = data;
                    this.dataLoaded = true;
            });
        }
    },
    mounted() {
        this.fetchArtistOfTheDay();
    }
}
</script>
