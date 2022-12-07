using Aboriginal_Art_Gallery_of_Australia.Models.Database_Models;
using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using System.Linq.Expressions;

namespace Aboriginal_Art_Gallery_of_Australia.Middleware
{
    class ArtistOfTheDayMiddleware
    {
        int i;
        Random rnd = new();
        List<ArtistOutputDto>? previousArtists;
        ArtistOutputDto? currentArtist;
        DateTime nextArtistAt, currentTime;

        public ArtistOutputDto? GetArtistOfTheDay(List<ArtistOutputDto> allArtists)
        {
            List<ArtistOutputDto>? eligibleArtists = allArtists;
            currentTime = DateTime.Now;

            if (previousArtists is null)
            {
                previousArtists = new();
                nextArtistAt = DateTime.Now;
            }
            else if (currentArtist is not null) previousArtists.Add(currentArtist);

            if (currentArtist is null || currentTime.CompareTo(nextArtistAt) >= 0)
            {
                if (eligibleArtists.Count is 0) return null;
                else eligibleArtists.RemoveAll(x => previousArtists.Exists(y => y.Id == x.Id));

                if (eligibleArtists.Count is 0) return null;
                //else if (eligibleArtists.Count is 1) previousArtists.Clear();

                int index = rnd.Next(eligibleArtists.Count);
                currentArtist = eligibleArtists[index];
                nextArtistAt.AddMinutes(10);
            }


            // I will leave the below here for debugging purposes, be sure to comment line 33 so you are not clearing the eligible artist list before printing it. :)
            i++;
            Console.WriteLine($"\nIteration: {i}");
            Console.WriteLine($"previousArtists:");
            foreach (ArtistOutputDto x in previousArtists)
                Console.WriteLine(x.Id);
            Console.WriteLine($"eligableAtists:");
            foreach (ArtistOutputDto x in eligibleArtists)
                Console.WriteLine(x.Id);
            Console.WriteLine($"currentArtist: {currentArtist.Id}");
            Console.WriteLine($"nextArtistAt: {nextArtistAt}");
            Console.WriteLine($"\n");
            if (eligibleArtists.Count is 1) previousArtists.Clear();
            /// End of debug code.
            return currentArtist;
        }
    }
}