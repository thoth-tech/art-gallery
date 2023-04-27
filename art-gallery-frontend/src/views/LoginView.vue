<template>
    <div class="container">
        <HeadingComponent title="Log in"/>
        <LoginComponent />
    </div>
</template>

<script>
import LoginComponent from '@/components/LoginComponent.vue'
import HeadingComponent from '@/components/HeadingComponent.vue'

export default ({
    name: 'LoginView',
    components: {
        HeadingComponent,
        LoginComponent
    }
})
</script>

<style scoped>
    :deep(.container) {
        text-align: center;
        position: relative;
    }
</style>
