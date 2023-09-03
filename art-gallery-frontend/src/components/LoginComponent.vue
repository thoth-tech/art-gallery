<template>
  <div class="container">
    <div class="card">
      <Form name="login-form" v-on:submit="handleLogin">
        <p class="login-text">
          Enter your login details to sign in, or click 'Sign up' to create an account.
        </p>

        <label for="email">Email:</label>
        <ErrorMessage name="email" class="error-message" />

        <Field
          v-model="email"
          type="email"
          placeholder="Email"
          class="form-control"
          v-validate="'required'"
        />

        <label for="password">Password:</label>
        <ErrorMessage name="password" class="error-message" />

        <Field
          v-model="password"
          type="password"
          placeholder="Password"
          class="form-control"
          v-validate="'required'"
        />

        <div class="button-div">
          <button type="submit" class="login-submit">Log In</button>
          <router-link to="/signup" class="btn-link">Sign up</router-link>
        </div>
      </Form>
    </div>
  </div>
</template>
<script>
import { mapState, mapActions } from "vuex";
import { Form, Field, ErrorMessage, defineRule } from "vee-validate";

// ... (rest of the script remains the same)
</script>

<style scoped>
.container {
  width: 90%;
  max-width: 600px;
  margin: 0 auto;
  margin-bottom: 15px;
}

.card {
  text-align: left;
  background-color: var(--color--grey-light);
  border-radius: 8px;
  border: 1px solid var(--color--grey);
  box-shadow: 1px 4px 4px rgba(0, 0, 0, 0.2);
  padding: 15px;
}

.login-text {
  color: var(--color--black);
  font-size: 0.9em;
  margin-top: 10px;
  font-family: var(--font--base);
}

label {
  font-family: var(--font--base);
  font-weight: var(--font--semibold);
  font-size: 0.9em;
  margin-left: 15px;
  display: block;
  margin-top: 10px;
}

.form-control {
  width: 100%;
  padding: 8px;
  margin: 5px 0;
  border: 1px solid var(--color--grey-med);
  border-radius: 3px;
  font-family: var(--font--base);
  font-size: 0.75em;
}

.form-control:focus {
  outline: var(--color--charcoal) solid 1px;
}

.button-div {
  margin-top: 20px;
  text-align: center;
}

.login-submit,
.btn-link {
  border: none;
  background-color: var(--color--primary);
  padding: 10px 20px;
  margin: 0 15px;
  font-weight: var(--font--semibold);
  border-radius: 5px;
  color: var(--color--white);
  text-decoration: none;
  cursor: pointer;
}

.login-submit:hover,
.btn-link:hover {
  background-color: var(--color--primary-hover);
  text-decoration: underline;
}

.error-message {
  font-size: 0.7em;
  margin-left: 20px;
  font-family: var(--font--base);
  color: var(--color--orange-dark);
}

@media only screen and (max-width: 600px) {
  .container {
    max-width: 350px;
  }
}
</style>
