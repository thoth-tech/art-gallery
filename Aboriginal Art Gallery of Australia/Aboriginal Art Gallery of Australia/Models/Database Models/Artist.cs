using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO;

namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    /// <summary>
    /// The Artist class is responsible for handling the database model associated with artists.
    /// </summary>
    public class Artist : IArtistDataAccess
    {
        public int ArtistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageURL { get; set; }
        public string PlaceOfBirth { get; set; }
        public int YearOfBirth { get; set; }
        public int? YearOfDeath { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Artist(int artistId, string firstName, string lastName, string displayName, string profileImageURL, string placeOfBirth, int yearOfBirth, int? yearOfDeath, DateTime modifiedAt, DateTime createdAt)
        {
            ArtistId = artistId;
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            ProfileImageURL = profileImageURL;
            PlaceOfBirth = placeOfBirth;
            YearOfBirth = yearOfBirth;
            YearOfDeath = yearOfDeath;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 39 is required for the active record implementation.
        private static string _connectionString = "Host=localhost;Database=Deakin University | AAGoA;Username=postgres;Password=postgreSQL;";

        public Artist() { }

        public List<ArtistOutputDto> GetArtists()
        {
            throw new NotImplementedException();
        }

        public ArtistOutputDto? GetArtistById(int id)
        {
            throw new NotImplementedException();
        }

        public ArtistInputDto? InsertArtist(ArtistInputDto artist)
        {
            throw new NotImplementedException();
        }

        public ArtistInputDto? UpdateArtist(int id, ArtistInputDto artist)
        {
            throw new NotImplementedException();
        }

        public bool DeleteArtist(int id)
        {
            throw new NotImplementedException();
        }
    }
}