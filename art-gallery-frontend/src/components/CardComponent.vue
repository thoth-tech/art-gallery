<template>
  <div class="card-div" v-if="heading">
    <h1 class="card-heading">{{ heading }}</h1>
    <img class="card-img" :src="imageURL" />
    <h2 class="card-subheading">{{ subheading }}</h2>
    <h4 class="detail">{{ detail1 }}</h4>
    <h4 class="detail">{{ detail2 }}</h4>
    <h4 class="detail">{{ detail3 }}</h4>
    <h4 class="detail-description">{{ detail4 }}</h4>
  </div>
</template>

<script>
export default {
  name: "CardComponent",
  props: {
    heading: {
      type: String,
    },
    imageURL: {
      type: String,
    },
    subheading: {
      type: String,
    },
    detail1: {
      type: String,
    },
    detail2: {
      type: String,
    },
    detail3: {
      type: String,
    },
    detail4: {
      type: String,
    },
  },
};
</script>

<style scoped>
.card-div {
  width: 70%;
  max-width: 900px;
  margin: auto;
  text-align: center;
  border: 1px solid var(--color--grey);
  border-radius: 10px;
  box-shadow: 10px 10px 10px var(--color--grey-light);
  font-family: var(--font--base);
  padding: 20px;
  padding-bottom: 40px;
  transition: opacity 0.5s ease-in-out;
}

.card-div.hidden {
  opacity: 0;
}
</style>
