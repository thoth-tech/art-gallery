<template>
    <div class="container">
        <HeadingComponent title="New Account" />
        <SignupComponent />
    </div>
</template>

<script>
import SignupComponent from '@/components/SignupComponent.vue'
import HeadingComponent from '@/components/HeadingComponent.vue'

export default ({
    name: 'SignupView',
    components: {
        HeadingComponent,
        SignupComponent
    }
})
</script>


<style scoped>
    :deep(.container) {
        text-align: center;
        position: relative;
    }
</style>
