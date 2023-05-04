---
title: Stored Data
---

### [ < Return Home ](./README.md)

# Stored data

## Account store

Handles actions relating to user account authentication, sending requests to the user service and changing the locally stored data in response.

Dispatches requests to the alert store when a user is successfully registered, or on receiving an error from the user service.

### actions

#### `login()`

Stores a record of the login request, and then a record of whether the login attempt was a success or a failure.

#### `logout()`

Stores a logout.

#### `register()`

Stores a request for registration, and then a record of whether the registration attempt was a success or a failure.

<br>
<br>

## Alert Store

Used by the Vue app to display alerts to the user regarding login or registration success or error.

### actions

#### `success()`

Displays a success message.

#### `error()`

Displays an error message.

#### `clear()`

Clears messages from the alert area.
