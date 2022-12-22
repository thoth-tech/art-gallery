using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;

namespace Aboriginal_Art_Gallery_of_Australia.Middleware
{
    public class ArtworkOfTheDayMiddleware
    {
        int i;
        Random rnd = new();
        List<ArtworkOutputDto>? previousArtworks;
        ArtworkOutputDto? currentArtwork;
        DateTime nextArtworkAt, currentTime;

        public ArtworkOutputDto? GetArtworkOfTheDay(List<ArtworkOutputDto> allArtworks)
        {
            List<ArtworkOutputDto>? eligibleArtworks = allArtworks;
            currentTime = DateTime.Now;

            if (previousArtworks is null)
            {
                previousArtworks = new();
                nextArtworkAt = DateTime.Now;
            }
            else if (currentArtwork is not null) previousArtworks.Add(currentArtwork);

            if (currentArtwork is null || currentTime.CompareTo(nextArtworkAt) >= 0)
            {
                if (eligibleArtworks.Count is 0) return null;
                else eligibleArtworks.RemoveAll(x => previousArtworks.Exists(y => y.ArtworkId == x.ArtworkId));

                if (eligibleArtworks.Count is 0) return null;
                //else if (eligibleArtworks.Count is 1) previousArtworks.Clear();

                int index = rnd.Next(eligibleArtworks.Count);
                currentArtwork = eligibleArtworks[index];
                nextArtworkAt = nextArtworkAt.AddMinutes(1);
            }


            // I will leave the below here for debugging purposes, be sure to comment line 33 so you are not clearing the eligible artworks list before printing it. :)
            i++;
            Console.WriteLine($"\nIteration: {i}");
            Console.WriteLine($"previousArtworks:");
            foreach (ArtworkOutputDto x in previousArtworks)
                Console.WriteLine(x.ArtworkId);
            Console.WriteLine($"eligibleArtworks:");
            foreach (ArtworkOutputDto x in eligibleArtworks)
                Console.WriteLine(x.ArtworkId);
            Console.WriteLine($"currentArtwork: {currentArtwork.ArtworkId}");
            Console.WriteLine($"nextArtworkAt: {nextArtworkAt}");
            Console.WriteLine($"\n");
            if (eligibleArtworks.Count is 1) previousArtworks.Clear();
            /// End of debug code.
            return currentArtwork;
        }
    }
}