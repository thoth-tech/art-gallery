using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;

namespace Aboriginal_Art_Gallery_of_Australia.Middleware
{
    /// <summary>
    /// The ArtistOfTheDayMiddleware class is responsible for handling the artist of the day selection.
    /// To change the delay between artists, alter the ArtistDuration constant which is in minutes.
    /// </summary>
    internal class ArtistOfTheDayMiddleware
    {
        private const int ArtistDuration = 1; // ArtistDuration is in minutes - i.e., 1 Day = 1440 minutes.
        private readonly Random rnd = new();
        private List<ArtistOutputDto>? previousArtists;
        private ArtistOutputDto? currentArtist;
        private DateTime nextArtistAt, currentTime;

        /// <summary>
        /// Cycles through the passed list of artists and selects a random artist every X minutes.
        /// </summary>
        /// <param name="allArtists">The list of all artists published by the gallery.</param>
        /// <returns>A random artist from the list of all artists.</returns>
        public ArtistOutputDto? GetArtistOfTheDay(List<ArtistOutputDto> allArtists)
        {
            List<ArtistOutputDto>? eligibleArtists = allArtists;
            currentTime = DateTime.Now;

            if (previousArtists is null)
            {
                previousArtists = new();
                nextArtistAt = DateTime.Now;
            }
            else if (currentArtist is not null)
            {
                previousArtists.Add(currentArtist);
            }

            if (currentArtist is null || currentTime.CompareTo(nextArtistAt) >= 0)
            {
                eligibleArtists.RemoveAll(x => previousArtists.Exists(y => y.ArtistId == x.ArtistId));
                
                if (eligibleArtists.Count <= 1)
                    previousArtists.Clear();

                if (eligibleArtists.Count is 0)
                    return null;

                int index = rnd.Next(eligibleArtists.Count);
                currentArtist = eligibleArtists[index];
                nextArtistAt = nextArtistAt.AddSeconds(ArtistDuration);
            }

            return currentArtist;
        }
    }
}