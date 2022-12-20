using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ArtistRepository : IRepository, IArtistDataAccess
    {
        // TODO: test last two methods

        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public ArtistRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public List<ArtistOutputDto> GetArtists()
        {
            var artists = _repo.ExecuteReader<ArtistOutputDto>("SELECT * FROM artist");
            return artists;
        }

        public ArtistOutputDto? GetArtistById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", id)
            };

            var artist = _repo.ExecuteReader<ArtistOutputDto>("SELECT * FROM artist WHERE artist_id=@artistId", sqlParams)
                .SingleOrDefault();

            return artist;
        }

        

        public ArtistInputDto? InsertArtist(ArtistInputDto artist)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("firstName", artist.FirstName),
                new("lastName", artist.LastName),
                new("displayName", artist.DisplayName),
                new("placeOfBirth", artist.PlaceOfBirth),
                new("yearOfBirth", artist.YearOfBirth),
                new("yearOfDeath", artist.YearOfDeath ?? (object)DBNull.Value)
            };

            var result = _repo.ExecuteReader<ArtistInputDto>("INSERT INTO artist " +
                "VALUES (DEFAULT, @firstName, @lastName, @displayName, @placeOfBirth, " +
                "@yearOfBirth, @yearOfDeath, current_timestamp, current_timestamp) RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public ArtistInputDto? UpdateArtist(int id, ArtistInputDto artist)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", id),
                new("firstName", artist.FirstName),
                new("lastName", artist.LastName),
                new("displayName", artist.DisplayName),
                new("placeOfBirth", artist.PlaceOfBirth),
                new("yearOfBirth", artist.YearOfBirth),
                new("yearOfDeath", artist.YearOfDeath ?? (object)DBNull.Value)
            };

            var result = _repo.ExecuteReader<ArtistInputDto>("UPDATE artist SET first_name = @firstName, " +
                "last_name = @lastName, display_name = @displayName, place_of_birth = @placeOfBirth, " +
                "year_of_birth = @yearOfBirth, year_of_death = @yearOfDeath, modified_at = current_timestamp " +
                "WHERE artist_id = @artistId RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public bool DeleteArtist(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", id)
            };

            _repo.ExecuteReader<ArtistOutputDto>("DELETE FROM artist WHERE artist_id = @artistId", sqlParams);
                
            return true;
        }

        // TODO: test method function
        public ArtistArtworkDto? AssignArtwork(int artistId, int artworkId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", artistId),
                new("artworkId", artworkId)
            };

            var result = _repo.ExecuteReader<ArtistArtworkDto>("INSERT INTO artist_artwork(artist_id, artwork_id, " +
                "modified_at, created_at) VALUES (@artistId, @artworkId, current_timestamp, " +
                "current_timestamp) RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        // TODO: test method function
        public bool DeassignArtwork(int artistId, int artworkId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", artistId),
                new("artworkId", artworkId)
            };

            _repo.ExecuteReader<ArtistArtworkDto>("DELETE FROM artist_artwork WHERE artist_id = @artistId " +
                "AND artwork_id = @artworkId", sqlParams);

            return true;
        }
    }
}