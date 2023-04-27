// Service to handle the auth header
export function authHeader() {
    let user = JSON.parse(localStorage.getItem('user'));

    if (user) {
      var token = user.token;
      token = token.replace(/^"(.*)"$/, '$1');
      return 'Bearer ' + token ;
    } else {
      return {};
    }
  }
