using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;

namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    /// <summary>
    /// The Artwork class is responsible for handling the database model associated with artworks. 
    /// </summary>
    public class Artwork : IArtworkDataAccess
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PrimaryImageURL { get; set; }
        public string SecondaryImageURL { get; set; }
        public int YearCreated { get; set; }
        public int MediaId { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Artwork(int artworkId, string title, string description, string primaryImageURL, string secondaryImageURL, int yearCreated, int mediaId, DateTime modifiedAt, DateTime createdAt)
        {
            ArtworkId = artworkId;
            Title = title;
            Description = description;
            PrimaryImageURL = primaryImageURL;
            SecondaryImageURL = secondaryImageURL;
            YearCreated = yearCreated;
            MediaId = mediaId;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 30 is required for the active record implementation.
        private static string _connectionString = "Host=localhost;Database=Deakin University | AAGoA;Username=postgres;Password=postgreSQL;";

        public Artwork() { }

        public List<ArtworkOutputDto> GetArtworks()
        {
            throw new NotImplementedException();
        }

        public ArtworkOutputDto? GetArtworkById(int id)
        {
            throw new NotImplementedException();
        }

        public ArtworkOutputDto? GetArtworkOfTheDay()
        {
            throw new NotImplementedException();
        }

        public ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork)
        {
            throw new NotImplementedException();
        }

        public ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork)
        {
            throw new NotImplementedException();
        }

        public bool DeleteArtwork(int id)
        {
            throw new NotImplementedException();
        }

        public ArtistArtworkDto? AllocateArtist(int artistId, int artworkId)
        {
            throw new NotImplementedException();
        }

        public bool DeallocateArtist(int artistId, int artworkId)
        {
            throw new NotImplementedException();
        }
    }
}