<template v-if="dataLoaded">
  <div class="exhibitions">
    <HeadingComponent title="EXHIBITIONS"/>
    <HeadingComponent title="Ant Design table using Vue's list and conditional rendering" class="subheading"/>
    <TableAntDesign :exhibitions="this.exhibitions" v-on:updateData="updateData()"/>
    <br>
    <br>
    <br>
    <HeadingComponent title="HTML table using Vue's list and conditional rendering" class="subheading"/>
    <TableExhibition :exhibitions="this.exhibitions" v-on:updateData="updateData()"/>
    <br>
    <br>
  </div>
</template>

<script>
import { getExhibitions } from '../services/ExhibitionService'
import TableExhibition from '../components/TableExhibition.vue';
import HeadingComponent from '@/components/HeadingComponent.vue';
import TableAntDesign from '@/components/TableAntDesign.vue';

export default {
  name: 'ExhibitionsView',
  components: {
    TableExhibition,
    HeadingComponent,
    TableAntDesign
  },
  data() {
    return  {
        exhibitions: [],
        dataLoaded: false,
        fields: []
    }
  },
  methods: {
        // Gets the data from endpoint and stores in an array.
        async fetchExhibitions() {
            await getExhibitions()
            .then(data => {
                this.exhibitions = data;
                this.fields = Object.keys(this.exhibitions[0]);
                this.parseDate();
                this.dataLoaded = true;
            });
        },
        // Update data on table item added.
        updateData() {
            this.fetchExhibitions();
        },
        // Changes the date strings to Date object.
        parseDate() {
          this.exhibitions.forEach(element => {
            var startDate = new Date(Date.parse(element.startDate));
            element.startDate = this.formatDate(startDate);
          });
          this.exhibitions.forEach(element => {
            var endDate = new Date(Date.parse(element.endDate));
            element.endDate = this.formatDate(endDate);
          });
        },
        // Formats Date object.
        formatDate(dateObject) {
          var date = dateObject.getDate();
          var month = dateObject.getMonth() + 1;
          var year = dateObject.getFullYear();

          if (date < 10) {
              date = '0' + date;
          }
          if (month < 10) {
              month = '0' + month;
          }
          return dateObject = date + "-" + month + "-" + year;
        }
    },
    mounted() {
        this.fetchExhibitions();
    },
}
</script>

<style scoped>
  .exhibitions {
    width: 70%;
    margin-bottom: 50px;
    margin:auto;
    margin-bottom: 110px;
  }

  .subheading {
    font-style: italic;
    font-weight:normal;
    color: var(--color--gray);
    font-size: max(25px);
  }

  @media only screen and (max-width: 600px) {
    .exhibitions {
      width: 95%;
    }
  }
</style>