<template>
    <h1 class="heading">{{ title }}</h1>
</template>

<script>
export default {
    name: "HeaderComponent",
    props: {
        title: {
            type: String
        }
    }
}
</script>

<style scoped>
    .heading {
        margin-top: 50px;
        text-align: center;
        font-family: var(--font--base);
        font-size: 40px;
        color: var(--color--charcoal);
        letter-spacing: 5px;
    }

    @media only screen and (max-width: 600px) {
        .heading {
            font-size: 30px;
        }
    }
</style>