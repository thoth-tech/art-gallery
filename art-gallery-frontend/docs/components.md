---
title: Components
---

### [ < Return Home ](./README.md)


# Components

## CardComponent

### Properties

| name | type | description
|---------|------------|---------|
| heading | string | The main heading of the card.
| imageURL | string | The image.
| subHeading | string | The subheading.
| detail1 | string | Text descriptions.
| detail2 | string | Text descriptions.
| detail3 | string | Text descriptions.
| detail4 | string | Text descriptions.

### Basic Usage

A basic card to display a heading, image, text etc.

::: tip
Card has a flexible-sized container so can be used to display many different types of information.
:::

<br>
<br>

## FeaturedArtist

### Properties

None

### Basic Usage

Gets the featured artist from the endpoint, stores in an array and displays this data in a card component.

### Components

CardComponen is used to display the featured artist data.

### Methods

#### `fetchArtistOfTheDay()`

Asynchronous method that fetches the artist of the day from the ArtistService's `getArtistOfTheDay`() method.

### Mounted

Ensures that artist of the day data has been fetched for display.

::: tip
`mounted()` is called after the component has been mounted.
:::

<br>
<br>

## FeaturedArtwork

### Properties

None

### Basic Usage

Gets the featured artwork from the endpoint, stores in an array and displays this data in a card component.

### Components

CardComponen is used to display the featured artwork data.

### Methods

#### `fetchArtworkOfTheDay()`

Asynchronous method that fetches the artist of the day from the ArtistService's `getArtworkOfTheDay`() method.

#### `getContributingArtists()`

Gets the contributing artists for the artwork and joins them with `, `.

### Mounted

Ensures that artwork of the day data has been fetched for display.

::: tip
`mounted()` is called after the component has been mounted.
:::

<br>
<br>

## FooterComponent

### Properties

None

### Basic Usage

Displays the contact information and socials in the footer.

### Components

SocialsComponent is used to display the site socials.

<br>
<br>

## HeaderComponent

### Properties

None

### Basic Usage

Displays the site logo and socials in the header.

### Components

SocialsComponent is used to display the site socials.

<br>
<br>

## HeadingComponent

### Properties

| name | type | description
|---------|-----------|-------|
| title | string | The text to be in the heading.

### Basic Usage

Used as the heading for each page with the title passed in as a prop.

<br>
<br>

## SignupComponent

### Properties

None

### Basic Usage

A signup form where users can sign up for an account.

### Components

Uses VeeValidate components for the form, fields, and error messages which appear when input is incorrect.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

### Methods

#### `mapActions()`

Maps the actions from the authentication service that can alter the state of the account, in order to log a user in or out.

#### `handleSubmit()`

Registers the new user.

<br>
<br>

## LoginComponent

### Properties

None

### Basic Usage

A login form where users can sign into their account, or be directed to the sign up page to register a new account.

### Components

Uses VeeValidate components for the form, fields, and error messages which appear when input is incorrect.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

### Methods

#### `mapActions()`

Maps the actions from the authentication service that can alter the state of the account, in order to log a user in or out.

#### `handleLogin()`

Passes the data in the email and password fields to the authentication service.

<br>
<br>

## NavigationComponent

### Properties

None

### Basic Usage

The navigation element for the page, allowing the user to navigate the site. It also displays a message to the user if there is a user logged in.

For narrow screen widths such as mobile devices, only a collapsible menu will display. For wider screens both a dropdown menu and a collapsible menu are used.

::: tip
You can quickly switch between the two designs (pure HTML and using Ant Design) by changing the `usingAntDesign` variable.
:::

### Components

* The LoginComponent is used to provide quick access for the user to login without navigating to the login page.
* DropdownMenuAntD and DropdownMenuHTML provide two options for the navigation dropdown menu, organised hierarchically.
* CollapsibleMenuAntD and CollapsibleMenuHTML are two options for the collapsible 'hamburger' style menu.

### Methods

#### `toggleLogin()`

Shows or hides the floating login window.

#### `toggleMenu()`

Shows or hides the collapsible menu.

#### `logout()`

Logs the user out using the authentication service.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

<br>
<br>

## CollapsibleMenuAntD

### Properties

None

### Basic Usage

The collapsible "hamburger" menu using AntDesign.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

<br>
<br>

## CollapsibleMenuHTML

### Properties

None

### Basic Usage

The collapsible "hamburger" menu using only HTML.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

<br>
<br>

## DropdownMenuAntD

### Properties

None

### Basic Usage

The dropdown navigation menu using AntDesign, with hierarchical links allowing users to navigate the site.

### Methods

#### `isAdmin()`

Returns true if a user is logged in and has the role 'Admin'.

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

<br>
<br>

## DropdownMenuHTML

### Properties

None

### Basic Usage

The dropdown navigation menu using only HTML, with links allowing users to navigate the site.

#### `isAdmin()`

Returns true if a user is logged in and has the role 'Admin'.

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

<br>
<br>

## SocialsComponent

### Properties

None

### Basic Usage

Displays links to the site social media sites as svg images, including a text version for accessibility. The current links point to the social media pages for Deakin University.

<br>
<br>

## TableAntDesign

### Properties

| name | type | description
|---------|-----------|-------|
| exhibitions | array | An array with the exhibition data.
| fields | array | An array with the exhibition data fields.

### Basic Usage

Displays exhibitions drawn from the database and allows admin to add exhibitions. Table is developed using Ant Design Vue and is sortable and filterable. Table also makes use of Vue's conditional rendering using `v-if` and `v-else`.

### Methods

#### `dateCompare(date1, date2)`

Compares two dates. Returns `1` if date1 is larger, `-1` is date1 is smaller and `0` if they are equal.

#### `isAdmin()`

Checks if user has the role of admin.

#### `showInputs()`

Displays they input fields to add an exhibition to the table.

#### `addExhibition()`

Posts an exhibition to the database and updates the table.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

<br>
<br>

## TableArtworks

### Properties

None


### Basic Usage

Displays artworks fetched from the database. This table is developed using only HTML and CSS and has a filter search bar above it.


### Methods

#### `fetchArtworks()`

Fetches all the artworks from the database by access the `getArtworks()` method in the ArtworkService.

#### `getContributingArtists()`

Gets the contributing artists for the artwork and joins them with `, `.

#### `fillTableWithData()`

Fills the table with data fetched from the database and calls `tableHover()` to allow hover effect on mouseover.

#### `tableHover()`

Handles the hover effect when mouseover the table.

#### `filterTable()`

Filters the table data to display artwork with name that matches typed in name.

### Mounted

Ensures that the artworks data has been fetched for display.

::: tip
`mounted()` is called after the component has been mounted.
:::

## TableExhibitions

### Properties

| name | type | description
|---------|----------|--------|
| exhibitions | array | An array with the exhibition data.
| fields | array | An array with the exhibition data fields.

### Basic Usage

Displays exhibitions fetched from the database. Table is developed using HTML, CSS and makes use of Vue's list rendering through `v-for` and Vue's conditional rendering using `v-if` and `v-else`. There is also a search bar above the table allowing for filtering.

### Methods

#### `isAdmin()`

Checks if user has the role of admin.

#### `filterTable()`

Filters the table data to display artwork with name that matches typed in name.

#### `showInputs()`

Displays they input fields to add an exhibition to the table.

#### `addExhibition()`

Posts an exhibition to the database and updates the table.

### Computed data

#### `mapState()`

Maps the current state of the account from the account store, to determine if there is a user logged in or not.

<br>
<br>

