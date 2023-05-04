import { createApp } from 'vue'
import App from '@/App.vue'
import router from '@/router/router'
import store from '@/store'
import '@/styles/main.css'
import Antd from "ant-design-vue";
import '@/styles/antd.css';

const app = createApp(App)
app.use(store)
app.use(router)
app.use(Antd)
app.mount('#app')
