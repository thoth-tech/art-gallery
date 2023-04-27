<template>
    <HeaderComponent />
    <NavigationComponent />
    <div class="alerts" v-if="alert.message" v-on:click="dismiss" :class="`alert ${alert.type}`">
        {{alert.message}}
        <p class="dismiss">Click to dismiss</p>
    </div>
    <router-view />
    <FooterComponent />
</template>

<script>
import { mapState, mapActions } from 'vuex'
import HeaderComponent from '@/components/HeaderComponent.vue';
import NavigationComponent from '@/components/NavigationComponent.vue';
import FooterComponent from '@/components/FooterComponent.vue';

export default {
    name: 'app',
        computed: {
            ...mapState({
                alert: state => state.alert
            })
        },
        methods: {
            dismiss(){
                this.clearAlert();
            },
            ...mapActions({
                clearAlert: 'alert/clear'
            })
        },
        watch: {
            // eslint-disable-next-line
            $route (to, from){
                // clear alert on location change
                this.clearAlert();
            }
        },
      components: {
    HeaderComponent,
    NavigationComponent,
    FooterComponent
}
};
</script>

<style scoped>
    .alerts {
        font-family: var(--font--base);
        font-style: italic;
        font-size: medium;
        color: var(--color--black);
        position: fixed;
        right: 20px;
        bottom: 30px;
        max-width: 600px;
        width: 40%;
        padding: 10px;
        background-color: rgba(233, 233, 233, 0.95);
        box-shadow: 5px 5px 3px rgba(0, 0, 0, 0.295);
        border-radius: 10px;
        margin: 0 auto;
    }
    .dismiss {
        font-size: small;
        text-align: right;
        margin:0;
    }
</style>
