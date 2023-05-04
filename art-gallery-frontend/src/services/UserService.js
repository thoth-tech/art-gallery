import VueJwtDecode from 'vue-jwt-decode';

// Service to handle logging in and out
export const userService = {
    Login,
    Logout,
    SignUp
};

async function Login(email, password) {
    const request = {
      method: "POST",
      headers: { "Content-Type": "application/json"},
      body: JSON.stringify({ email: email, password: password})
    };

    await fetch('/api/users/login/', request)
        .then(handleLoginResponse)
        .then(data => {
            const decoded = VueJwtDecode.decode(data);
            localStorage.setItem('user',
            JSON.stringify({
                token: data,
                name: decoded.given_name,
                expiry: decoded.exp,
                role: decoded.role
            })
            );
            return data
        })
}

function Logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
    location.reload(true);
}

function handleLoginResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                Logout();
            }
            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }
        return data;
    });
}

// Register a new account using the sign up api
export async function SignUp(firstName, lastName, email, password) {
    const request = {
      method: "POST",
      headers: { "Content-Type": "application/json"},
      body: JSON.stringify({ firstName: firstName, lastName:lastName, email: email, password: password})
    }
    await fetch('/api/users/signup/', request)
        .then(handleSignupResponse);
}

function handleSignupResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                Logout();
            }

            const error = (data) || response.statusText;
            return Promise.reject(error);
        }
        return data;
    });
}
