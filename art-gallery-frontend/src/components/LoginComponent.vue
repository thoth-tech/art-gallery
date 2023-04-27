<template>
    <div class="container">
        <div class="card">
            <Form name="login-form" @submit="handleLogin">
                <p class="login-text">Enter your login details to sign in, or click 'Sign up' to create an account.</p>

                <label for="email">Email</label>
                <p class='error-message'><ErrorMessage name="email" /></p>
                <Field v-model="email" type="email" placeholder="Email" class="form-control" name="email" rules="required" />

                <label for="password">Password:</label>
                <p class='error-message'><ErrorMessage name="password" /></p>
                <Field v-model="password" type="password" placeholder="Password" name="password" class="form-control" rules="required"/>

                <div class="button-div">
                    <button type="submit" class="login-submit">Log In</button>
                    <button><router-link to="/signup" class="btn-link">Sign up</router-link></button>
                </div>
            </Form>
        </div>
    </div>
</template>

<script>
import { mapState, mapActions } from 'vuex'
import { Form, Field, ErrorMessage, defineRule } from 'vee-validate';

defineRule("required", (value) => {
    if (!value) {
      return "This field is required";
    }

    return true;
});

export default {
    name: 'LoginComponent',
    data() {
        return {
            submitted: false,
            email:"",
            password:""
        };
    },
    components: {
        Form,
        Field,
        ErrorMessage
    },
    computed: {
        ...mapState('account', ['status'])
    },
    methods: {
        ...mapActions('account', ['login', 'logout']),
        handleLogin () {
            this.submitted = true;
            const { email, password } = this;
            if (email && password) {
                this.login({ email, password })
            }
        }
    }
};
</script>

<style scoped>
    .container {
        width: 90vw;
        margin-left: 5%;
        margin-right: 5%;
        margin-bottom: 15px;
        position: absolute;
        z-index: 1000;
    }

    .card {
        text-align: left;
        max-width: 550px;
        display: inline-block;
        background-color: var(--color--grey-light);
        border-radius: 8px;
        border: 1px solid var(--color--grey);
        box-shadow: 1px 4px 4px rgba(0, 0, 0, 0.2);
        padding: 10px 15px 15px;
    }

    h3 {
        margin: 0;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    }

    input[type=text], input[type=email], input[type=password] {
        width: 80%;
        padding: 5px;
        margin: 10px;
        display: inline-block;
        border: var(--color--grey-med) solid 1px;
        border-radius: 3px;
        font-family: var(--font--base);
        font-size: .75em;
    }

    input[type=text]:focus, input[type=email]:focus, input[type=password]:focus {
        outline: var(--color--charcoal) solid 1px;
    }

    .login-text {
        color: var(--color--black);
        font-size: .9em;
        padding: 0;
        margin: 10px 0 0 0;
        font-family: var(--font--base);
    }

    label {
        font-family: var(--font--base);
        font-weight: var(--font--semibold);
        font-size: .9em;
        margin-left: 15px;
    }

    .button-div {
        margin-top: 20px;
        margin-bottom: 15px;
        text-align: center;
    }

    button {
        border: none;
        background-color: var(--color--primary);
        padding: 10px 20px;
        margin: 0 15px;
        font-weight: var(--font--semibold);
        border-radius: 5px;
    }

    button, .btn-link {
        color: var(--color--white);
        text-decoration: none;
    }

    button:hover {
        background-color: var(--color--primary-hover);
        text-decoration: underline;
        cursor: pointer;
    }

    .password-prompt, .error-message {
        font-size: .7em;
        padding-left: 20px;
        margin: 0;
    }
    .error-message {
        font-family: var(--font--base);
        color: var(--color--orange-dark);
    }

    @media only screen and (max-width: 600px) {
        .container {
            max-width: 350px;
        }
    }
</style>