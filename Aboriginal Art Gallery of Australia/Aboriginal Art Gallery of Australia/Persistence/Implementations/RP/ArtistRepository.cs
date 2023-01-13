using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ArtistRepository : IRepository, IArtistDataAccess
    {
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
                new("profileImageURL", artist.ProfileImageUrl),
                new("placeOfBirth", artist.PlaceOfBirth),
                new("yearOfBirth", artist.YearOfBirth),
                new("yearOfDeath", artist.YearOfDeath ?? (object)DBNull.Value)
            };

            var result = _repo.ExecuteReader<ArtistInputDto>("INSERT INTO artist " +
                "VALUES (DEFAULT, @firstName, @lastName, @displayName, @profileImageURL, @placeOfBirth, " +
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

            String cmdString = "UPDATE artist SET ";

            if (artist.FirstName is not null && artist.FirstName != "" && artist.FirstName != "string")
            {
                cmdString += "first_name = @firstName, ";
            }
            if (artist.LastName is not null && artist.LastName != "" && artist.LastName != "string")
            {
                cmdString += "last_name = @lastName, ";
            }
            if (artist.DisplayName is not null && artist.DisplayName != "" && artist.DisplayName != "string")
            {
                cmdString += "display_name = @displayName, ";
            }
            if (artist.ProfileImageUrl is not null && artist.ProfileImageUrl != "" && artist.ProfileImageUrl != "string")
            {
                cmdString += "profile_image_url = @profileImageURL, ";
            }
            if (artist.PlaceOfBirth is not null && artist.PlaceOfBirth != "" && artist.PlaceOfBirth != "string")
            {
                cmdString += "place_of_birth = @placeOfBirth, ";
            }
            if (artist.YearOfBirth is not null && artist.YearOfBirth != 0)
            {
                cmdString += "year_of_birth = @yearOfBirth, ";
            }
            if (artist.YearOfDeath is not null && artist.YearOfDeath != 0)
            {
                cmdString += "year_of_death = @yearOfDeath, ";
            }

            cmdString += "modified_at = current_timestamp WHERE artist_id = @artistId RETURNING *";

            var result = _repo.ExecuteReader<ArtistInputDto>(cmdString, sqlParams).SingleOrDefault();

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

    }
}