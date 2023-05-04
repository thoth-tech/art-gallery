import { userService } from '@/services/UserService'
import router from '@/router/router'

const user = JSON.parse(localStorage.getItem('user'));
const state = user
    ? { status: { loggedIn: true }, user }
    : { status: {}, user: null };

const actions = {
    login({ dispatch, commit }, { email, password }) {
        commit('loginRequest', { email });
        userService.Login(email, password)
            .then(
                user => {
                    commit('loginSuccess', user);

                    location.reload(true);
                    router.push('/')
                },
                error => {
                    router.push('/login')
                    commit('loginFailure', error);
                    setTimeout(() => {
                        // display success message after route change completes
                        dispatch('alert/error', 'Login failed: ' + error, { root: true });
                    })
                }
            );
    },
    logout({ commit }) {
        userService.Logout();
        commit('logout');
    },
    register({ dispatch, commit }, user) {
        commit('registerRequest', user);
        userService.SignUp(user.firstName,user.lastName,user.email, user.password)
            .then(
                user => {
                    commit('registerSuccess', user);

                    router.push('/login')

                    setTimeout(() => {
                        // display success message after route change completes
                        dispatch('alert/success', 'Registration successful', { root: true });
                    })
                },
                error => {
                    commit('registerFailure', error);
                    dispatch('alert/error', 'Registration failed: ' + error, { root: true });
                }
            );
    }
};

const mutations = {
    loginRequest(state, user) {
        state.status = { loggingIn: true };
        state.user = user;
    },
    loginSuccess(state, user) {
        state.status = { loggedIn: true };
        state.user = user;
    },
    loginFailure(state) {
        state.status = {};
        state.user = null;
    },
    logout(state) {
        state.status = {};
        state.user = null;
    },
    // eslint-disable-next-line
    registerRequest(state, user) {
        state.status = { registering: true };
    },
    // eslint-disable-next-line
    registerSuccess(state, user) {
        state.status = {};
    },
    // eslint-disable-next-line
    registerFailure(state, error) {
        state.status = {};
    }
};

export const account = {
    namespaced: true,
    state,
    actions,
    mutations
};
