<template>
    <footer>
        <br />
        <br />
        <br />
        <div class="footer-component">
            <div class="footer-links">
                <ul class="footer-nav">
                    <li>
                        <router-link to="/about">About Us</router-link>
                    </li>
                    <li>
                        <router-link to="/contact">Contact Us</router-link>
                    </li>
                </ul>
                <p>
                    Copyright © 2022 XYZ Gallery Pty Ltd.
                    <br>
                    <span>
                        Phone:
                        <a href="tel://(03) 9244 6333">(03) 9244 6333</a>
                    </span>
                    <span>
                        World:
                        <a href="tel://+61 3 9244 6333">+61 3 9244 6333</a>
                    </span>
                    <br>
                    <span>221 Burwood Highway, Burwood</span>
                    <span>VIC 3125 Australia</span>
                </p>
            </div>
            <div class="footer-socials">
                <SocialsComponent />
            </div>
        </div>
    </footer>
</template>

<script>
import SocialsComponent from '@/components/SocialsComponent.vue'
export default {
    components: { SocialsComponent }
}
</script>

<style scoped>
    :deep(.footer-socials ul){
        list-style: none;
        text-align: right;
        margin-top:1em;
    }

    :deep(.social) {
        margin-right: calc(2px + 2 * (100vw - 320px) / 1040);
        display: inline-block;
    }

    :deep(.footer-socials .socials-svg) {
        height: 34px;
        width: 34px;
        fill: var(--color--black);
        fill-rule: evenodd;
    }

    a {
        outline: none;
        text-decoration: none;
    }

    .footer-component {
        display:flex;
        font-family: var(--font--base);
        font-weight: var(--font--semibold);
        font-size: .85em;
        margin: 5px 5px 5px 15px;
    }

    .footer-links, .footer-socials {
        width: 50%;
    }

    .footer-nav{
        list-style: none;
        display: inline-flex;
        padding: 0;
        transition: ease all .5s;
    }

    .footer-nav li {
        padding-right:20px;
    }

    .footer-links a, .footer-links a:visited {
        color: var(--color--grey-charcoal);
        font-weight: var(--font--normal);
    }
    .footer-links span, .footer-links p {
        text-align: left;
        color: var(--color--grey-dark);
        line-height: 150%;
    }
    .footer-links span {
        padding-right: 10px;
    }
    :deep(svg:hover), :deep(.footer-socials svg:hover) {
        transition: .1s;
        transition-delay: 0;
    }

    :deep(.facebook:hover) {
        fill:var(--color--facebook);
    }

    :deep(.instagram:hover) {
        fill:var(--color--instagram);
    }

    :deep(.twitter:hover) {
        fill:var(--color--twitter);
    }

    :deep(.youtube:hover) {
        fill:var(--color--youtube);
    }

    :deep(.linkedin:hover) {
        fill:var(--color--linkedin);
    }

    @media only screen and (max-width: 600px) {
        .footer-component {
            display:inline;
            font-size: .9em;
        }
        .footer-nav {
            display: block;
            line-height: 180%;
        }
        .footer-links, :deep(.footer-socials) {
            padding-left: 10px;
            width: 100%;
        }
        :deep(.footer-socials ul) {
            text-align: left;
            padding-left: 0;
        }
    }
</style>
