import { createStore } from 'vuex'

import { alert } from '@/store/alert.module';
import { account } from '@/store/account.module';


export default createStore({
  state: {
  },
  getters: {
  },
  mutations: {
  },
  actions: {
  },
  modules: {
    alert,
    account
  }
})