import { authHeader } from '@/services/AuthService'

// Fetch exhibitions from database.
export async function getExhibitions() {
  const response = await fetch('/api/exhibitions')
  return await response.json();
}

// Post exhibition to database.
export async function postExhibition(name, description, backgroundImageUrl, startDate, endDate) {
  const request = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: authHeader()
    },
    body: JSON.stringify({ name: name, description: description, backgroundImageUrl: backgroundImageUrl, startDate: startDate, endDate: endDate})
  }
    let response = await fetch('/api/exhibitions', request)
    let responseJSON = response.json()

    return responseJSON
}
