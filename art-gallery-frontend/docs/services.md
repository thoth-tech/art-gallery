---
title: Services
---

### [ < Return Home ](./README.md)

# Services

## ArtistService

### Methods

#### `getArtists()`

Fetches all of the artists from the database.

#### `getArtistOfTheDay()`

Fetches the artist of the day from the database.

## ArtworkService

### Methods

#### `getArtworks()`

Fetches all of the artworks from the database.

#### `getArtworkOfTheDay()`

Fetches the artwork of the day from the database.

## AuthService

Used to create the auth header for API requests which require authentication.

## ExhibitionService

### Methods

#### `getExhibitions()`

Fetches all of the exhibitions from the database.

#### `postExhibition()`

Posts an exhibition to the database.

## UserService

* Handles user login and registration requests by sending API requests to the relevant end points.
* Receives token data and stores relevant information in the account store.

### Methods

#### `Login()`

Authenticates user information gathered using the LoginComponent's form data, and creates user data if login is successful, including:
* The token, for making authenticated requests
* The user's given name for displaying a message in the navigation component
* The expiry of the token to enable logout when tokens expire
* The user's role

#### `Logout()`

Logs a user out by deleting the stored user information from local storage and reloading the page.

#### `handleLoginResponse()`

Receives the request from the Login() method, sends it to the API and handles the data received in the API response.

#### `SignUp()`

Submits user information gathered using the registration form data.

#### `handleSignupResponse()`

Receives the request from the SignUp() method, sends it to the API and handles the data received in the API response.

<br>
<br>
