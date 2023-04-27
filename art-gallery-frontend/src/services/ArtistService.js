// Fetch artists from database.
export async function getArtists() {
  const response = await fetch('/api/artists')
  return await response.json();
}

// Fetch artist of the day.
export async function getArtistOfTheDay() {
  const response = await fetch('/api/artists/of-the-day')
  return await response.json();
}
