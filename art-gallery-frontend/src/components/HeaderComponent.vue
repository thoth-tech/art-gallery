<template>
    <header class="header-component">
        <div class="header flex-spread">
            <div class="header-logo">
                <router-link class="site-logo" to="/" aria-label="Gallery Home Page">
                    <span class="screen-reader-only">Site Logo</span>
                    <svg xmlns="http://www.w3.org/2000/svg" class="site-logo-svg" viewBox="0 0 320 80">
                        <rect id="logo=bg" class="logo-bg" width="320" height="80"/>
                        <text id="logo-text" kerning="auto" x="16px" y="54px"><tspan class="logo-text">DDGIT&#32;Gallery</tspan></text>
                    </svg>
                </router-link>
            </div>
            <div class="header-socials">
                <SocialsComponent />
            </div>
        </div>
    </header>
</template>

<script>
import SocialsComponent from '@/components/SocialsComponent.vue'
export default {
    components: { SocialsComponent }
}
</script>

<style scoped>
    .header-logo{
        margin-top: 5px;
        max-width: 250px;
    }

    .site-logo-svg {
        width: 100%;
    }

    .logo-bg {
        fill:var(--color--black);
    }

    .logo-text{
        font-size:44px;
        font-family: var(--font--base);
        font-weight: var(--font--semibold);
        fill:var(--color--grey-light);
    }

    :deep(.header-socials ul) {
        list-style: none;
        text-align: right;
        margin-top: 10px;
        margin-right:5px;
    }

    :deep(.social) {
        margin-right: calc(2px + 2 * (100vw - 320px) / 1040);
        display: inline-block;
    }

    :deep(.header-socials .socials-svg) {
        height: 24px;
        width: 24px;
        fill:var(--color--black);
        fill-rule: evenodd;
    }

    a {
        outline: none;
        text-decoration: none;
    }

    .header-logo:hover .logo-bg{
        transition: .3s;
        transition-delay: 0;
        fill:var(--color--turquoise-light);
    }

    :deep(svg:hover), :deep(.header-socials svg:hover) {
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
        :deep(.header-socials) {
            display: none;
        }
    }
</style>
