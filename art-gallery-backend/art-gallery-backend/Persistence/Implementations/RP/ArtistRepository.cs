using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence.Interfaces;
using static Art_Gallery_Backend.Persistence.ExtensionMethods;
using Npgsql;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Art_Gallery_Backend.Persistence.Implementations.RP
{
    public class ArtistRepository : IRepository, IArtistDataAccessAsync
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public ArtistRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        public async Task<List<ArtistOutputDto>> GetArtistsAsync()
        {
            var artists = await _repo.ExecuteReaderAsync<ArtistOutputDto>("SELECT * FROM artist");
            return artists;
            // return Task.FromResult(artists); ???
        }

        // For ArtistOfTheDay Middleware
        public List<ArtistOutputDto> GetArtists()
        {
            var artists = _repo.ExecuteReader<ArtistOutputDto>("SELECT * FROM artist");
            return artists;
        }

        public async Task<ArtistOutputDto?> GetArtistByIdAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", id)
            };

            var artists = await _repo.ExecuteReaderAsync<ArtistOutputDto>("SELECT * FROM artist WHERE artist_id=@artistId", sqlParams);
            var artist = artists.SingleOrDefault();

            return artist;
        }

        public async Task<ArtistInputDto?> InsertArtistAsync(ArtistInputDto artist)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("firstName", textInfo.ToTitleCase(artist.FirstName)),
                new("lastName", textInfo.ToTitleCase(artist.LastName)),
                new("displayName", textInfo.ToTitleCase(artist.DisplayName)),
                new("profileImageURL", artist.ProfileImageUrl),
                new("placeOfBirth", textInfo.ToTitleCase(artist.PlaceOfBirth)),
                new("yearOfBirth", artist.YearOfBirth),
                new("yearOfDeath", artist.YearOfDeath ?? (object)DBNull.Value)
            };

            var results = await _repo.ExecuteReaderAsync<ArtistInputDto>("INSERT INTO artist " +
                "VALUES (DEFAULT, @firstName, @lastName, @displayName, @profileImageURL, @placeOfBirth, " +
                "@yearOfBirth, @yearOfDeath, current_timestamp, current_timestamp) RETURNING *", sqlParams);

            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<ArtistInputDto?> UpdateArtistAsync(int id, ArtistInputDto artist)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", id),
                new("firstName", textInfo.ToTitleCase(artist.FirstName)),
                new("lastName", textInfo.ToTitleCase(artist.LastName)),
                new("displayName", textInfo.ToTitleCase(artist.DisplayName)),
                new("profileImageURL", artist.ProfileImageUrl),
                new("placeOfBirth", textInfo.ToTitleCase(artist.PlaceOfBirth)),
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
            if (artist.PlaceOfBirth is not null && artist.PlaceOfBirth != "" && artist.PlaceOfBirth != "string")
            {
                cmdString += "place_of_birth = @placeOfBirth, ";
            }
            if (artist.ProfileImageUrl is not null && artist.ProfileImageUrl != "" && artist.ProfileImageUrl != "string")
            {
                cmdString += "profile_image_url = @profileImageURL, ";
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

            var results = await _repo.ExecuteReaderAsync<ArtistInputDto>(cmdString, sqlParams);
            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<bool> DeleteArtistAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", id)
            };

            //TODO: add logic between 134 and 135 to check return value of of execute reader -> if 1 return true if 0 return false
            await _repo.ExecuteReaderAsync<ArtistOutputDto>("DELETE FROM artist WHERE artist_id = @artistId", sqlParams);
            return true;
        }
    }
}
