<template>
  <footer>
    <div class="footer-component">
      <div class="footer-links">
        <ul class="footer-nav">
          <li>
            <router-link to="about">About Us</router-link>
          </li>
          <li>
            <router-link to="contact">Contact Us</router-link>
          </li>
        </ul>
        <address>
          <p>
            &copy; 2022 XYZ Gallery Pty Ltd.<br />
            <span>Phone: <a href="tel:(03) 9244 6333">(03) 9244 6333</a></span><br />
            <span>World: <a href="tel:+61 3 9244 6333">+61 3 9244 6333</a></span><br />
            <span>221 Burwood Highway, Burwood</span><br />
            <span>VIC 3125 Australia</span>
          </p>
        </address>
      </div>
      <div class="footer-socials">
        <SocialsComponent />
      </div>
    </div>
  </footer>
</template>

<script>
import SocialsComponent from '@/components/SocialsComponent.vue';

export default {
  components: {
    SocialsComponent
  }
};
</script>

<style scoped>
  .footer-component {
    display: flex;
    font-family: var(--font--base);
    font-weight: var(--font--semibold);
    font-size: 0.85em;
    margin: 1em;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
  }

  .footer-links {
    flex: 1;
    padding-right: 1em;
  }

  .footer-socials {
    flex: 1;
    display: flex;
    justify-content: flex-end;
  }

  .footer-nav {
    list-style: none;
    display: flex;
    padding: 0;
    transition: ease all 0.5s;
  }

  .footer-nav li {
    margin-right: 20px;
  }

  /* Add your other styles here */

  @media only screen and (max-width: 600px) {
    .footer-component {
      flex-direction: column;
    }

    .footer-links,
    .footer-socials {
      width: 100%;
    }

    /* Add your responsive styles here */
  }
</style>
