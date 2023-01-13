using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;

namespace Aboriginal_Art_Gallery_of_Australia.Middleware
{
    /// <summary>
    /// The ArtworkOfTheDayMiddleware class is responsible for handling the artwork of the day selection.
    /// To change the delay between artworks, alter the ArtworkDuration constant which is in minutes.
    /// </summary>
    public class ArtworkOfTheDayMiddleware
    {
        private const int ArtworkDuration = 1; // ArtworkDuration is in minutes - i.e., 1 Day = 1440 minutes.
        private readonly Random rnd = new();
        private List<ArtworkOutputDto>? previousArtworks;
        private ArtworkOutputDto? currentArtwork;
        private DateTime nextArtworkAt, currentTime;

        /// <summary>
        /// Cycles through the passed list of artworks and selects a random artwork every X minutes. 
        /// </summary>
        /// <param name="allArtworks">The list of all artworks published by the gallery.</param>
        /// <returns>A random artwork from the list of all artworks.</returns>
        public ArtworkOutputDto? GetArtworkOfTheDay(List<ArtworkOutputDto> allArtworks)
        {
            List<ArtworkOutputDto>? eligibleArtworks = allArtworks;
            currentTime = DateTime.Now;

            if (previousArtworks is null)
            {
                previousArtworks = new();
                nextArtworkAt = DateTime.Now;
            }
            else if (currentArtwork is not null)
            {
                previousArtworks.Add(currentArtwork);
            }

            if (currentArtwork is null || currentTime.CompareTo(nextArtworkAt) >= 0)
            {
                if (eligibleArtworks.Count is 0)
                {
                    return null;
                }
                else
                {
                    _ = eligibleArtworks.RemoveAll(x => previousArtworks.Exists(y => y.ArtworkId == x.ArtworkId));
                }

                if (eligibleArtworks.Count is 0)
                {
                    return null;
                }

                int index = rnd.Next(eligibleArtworks.Count);
                currentArtwork = eligibleArtworks[index];
                nextArtworkAt = nextArtworkAt.AddMinutes(ArtworkDuration);
            }

            return currentArtwork;
        }
    }
}