---
title: Views
---

### [ < Return Home ](./README.md)

# Views

## AboutView

Contains the site author information.

### Components

`HeadingComponent` is used to display the page heading.

<br>
<br>

## ArtistDayView

Displays the featured artist of the day.

### Components

`HeadingComponent` is used to display the page heading.

`FeaturedArtist` is a card which displays the featured artist of the day data.

<br>
<br>

## ArtworkDayView

Displays the featured artwork of the day.

### Components

`HeadingComponent` is used to display the page heading.

`FeaturedArtwork` is a card which displays the featured artwork of the day data.

<br>
<br>

## Artworks

Displays the artworks table.

### Components

`HeadingComponent` is used to display the page heading.

`TableArtworks` is a HTML table displaying the artworks data.

<br>
<br>

## ContactView

A blank view for adding contact information or a contact form.

### Components

`HeadingComponent` is used to display the page heading.

<br>
<br>

## CultureView

Displays the artist of the day and artwork of the day.

### Components

`HeadingComponent` is used to display the page main heading and subheadings.

`FeaturedArtist`  is a card which displays the featured artist of the day data.

`FeaturedArtwork`  is a card which displays the featured artwork of the day data.

<br>
<br>

## ExhibitionsView

Displays both exhibition tables.

### Components

`HeadingComponent` is used to display the page main heading and subheadings.

`TableExhibition` is a HTML table using Vue's list and conditional rendering.

`TableAntDesign` is an Ant Design table using Vue's list and conditional rendering.

### Methods

#### `fetchExhibitions()`

Asynchronous method which uses the ExhibitionService to fetch the exhibitions from the database.

#### `updateData()`

Updates the data in the table when an exhibition item is added.

#### `parseDate()`

Changes date strings to Date objects.

#### `formatDate`

Formats a Date object to display the date as 'DD/MM/YYYY'.

### Mounted

Ensures that the artworks data has been fetched for display.

::: tip
`mounted()` is called after the component has been mounted.
:::

<br>
<br>

## HomeView

Home page displaying a splash image (open access artwork) and a message to the user if one is logged in.

### Components

`HeadingComponent` is used to display the page heading.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

<br>
<br>

## LoginView

User login view.

### Components

`HeadingComponent` is used to display the page heading.
`LoginComponent` is the login form, used to submit the user's account information.

<br>
<br>

## SignUpView

User registration view, for new users.

The sign up form fields use `vee-validate` to validate the user input before it is sent to the back end. The custom rules used for validation are:

| name | description |
|---------|------------------|
| required | Form will not submit without a given value
| min | Field has a minimum character requirement
| max | Field has a maximum character requirement
| valid-name | Uses a regex to check for a valid name input
| valid-email | Uses a regex to check for a valid email input


### Components

`HeadingComponent` is used to display the page heading.
`Form` is used to create the sign up form.
`Field` is the form input fields.
`ErrorMessage` is used to display custom errors for each form field based on the custom rules given above.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

### Methods

#### back()

Returns to the home page.

#### handleSubmit()

Handles sending the user information given in the sign up form to the account service.

