// Fetch artworks from database.
export async function getArtworks() {
  const response = await fetch('/api/artworks')
  return await response.json();
}

// Fetch artwork of the day.
export async function getArtworkOfTheDay() {
  const response = await fetch('/api/artworks/of-the-day')
  return await response.json();
}