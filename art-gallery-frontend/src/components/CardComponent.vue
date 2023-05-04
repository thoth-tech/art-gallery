<template>
    <div class="card-div">
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
            type: String
        },
        imageURL: {
            type: String
        },
        subheading: {
            type: String
        },
        detail1: {
            type: String
        },
        detail2: {
            type: String
        },
        detail3: {
            type: String
        },
        detail4: {
            type: String
        }
    }
}
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
        font-family:var(--font--base);
        padding: 20px;
        padding-bottom: 40px;
    }

    .card-img {
        width:90%;
        max-height: 500px;
        object-fit: cover;
        padding-top: 10px;
    }

    .card-heading {
        font-family:var(--font--base);
        letter-spacing: 4px;
        color: var(--color--charcoal);
        padding-top: 20px;
        font-weight:lighter;
        font-size: 35px;
    }

    .card-subheading {
        font-family:var(--font--base);
        letter-spacing: 4px;
        color: var(--color--charcoal);
        padding-top: 20px;
        font-size: 35px;
    }

    .detail, .detail-description {
        font-family:var(--font--base);
        letter-spacing: .5px;
        color: var(--color--charcoal);
        font-size: 18px;
    }

    .detail-description {
        text-align: justify;
        font-size: 15px;
    }

    @media only screen and (max-width: 800px) {
        .card-div {
            width: 90%;
            padding: 0px;
            padding-bottom: 10px;
        }

        .card-subheading {
            font-size: 20px;
            letter-spacing: 2px;
        }

        .detail, .detail-description {
            font-size: 16px;
            letter-spacing: .5px;
        }

    }

</style>