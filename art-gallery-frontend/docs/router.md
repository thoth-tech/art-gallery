---
title: Router
---

### [ < Return Home ](./README.md)
# Router

Creates a router with a web history and assigns a path and name to each of the view components.

Before each route is loaded, a navigation guard checks the local storage for a user logged in with a valid token.

* If a valid token is found, no action
* If the token is found to be expired, the router uses the user service to log out the user.

## Reference

Vue Router API:
https://router.vuejs.org/api/interfaces/Router.html

