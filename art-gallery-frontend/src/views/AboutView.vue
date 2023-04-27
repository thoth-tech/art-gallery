<template>
    <HeadingComponent title="About"/>

    <div class="about-text">
      <p>
        Created for the Deakin Design Graduate Certificate in IT course by:
        <ul>
          <li>Luke Collins</li>
          <li>Sarah Gosling</li>
          <li>Chloe Hulme</li>
          <li>Hannah Smith</li>
        </ul>
      </p>
    </div>
</template>

<script>
import HeadingComponent from '@/components/HeadingComponent.vue';

export default {
  name: 'AboutView',
  components: {
    HeadingComponent
  }
}
</script>


<style scoped>
  .about-text {
      color: var(--color--charcoal);
      font-size: larger;
      position: relative;
      padding: 0 100px;
      font-weight: var(--font--semibold);
  }

  .about-text ul {
    font-size: medium;
    font-weight: var(--font--normal);
  }
</style>