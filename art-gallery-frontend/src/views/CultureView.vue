<template>
  <div class="culture">
    <HeadingComponent title="ART & CULTURE" />
    <HeadingComponent title="Symbols" class="subheading" />
    <br />
    <br />
    <HeadingComponent title="Art & Culture Facts" class="subheading" />
  </div>
</template>

<script>
import HeadingComponent from "@/components/HeadingComponent.vue";

export default {
  name: "CultureView",
  components: {
    HeadingComponent,
  },
};
</script>

<style scoped>
.subheading {
  font-style: italic;
  font-weight: normal;
  color: var(--color--gray);
}
</style>
