using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence.Interfaces;
using static Art_Gallery_Backend.Persistence.ExtensionMethods;
using Npgsql;
using System.Globalization;
using Art_Gallery_Backend.Models.Database_Models;

namespace Art_Gallery_Backend.Persistence.Implementations.RP
{
    public class ArtworkRepository : IRepository, IArtworkDataAccessAsync
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public ArtworkRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        public async Task<List<ArtworkOutputDto>> GetArtworksAsync()
        {
            var allArtworks = new List<ArtworkOutputDto>();
            var allArtworkArtists = new List<KeyValuePair<int, String>>();

            var artworks = await _repo.ExecuteReaderAsync<ArtworkOutputDto>("SELECT artwork_id " +
                "FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id");

            var artists = await _repo.ExecuteReaderAsync<ArtistOutputDto>("SELECT display_name FROM artist_artwork " +
                "INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id");

            int i = 0;
            foreach (ArtworkOutputDto artwork in artworks)
            {
                allArtworkArtists.Add(new KeyValuePair<int, String>(artwork.ArtworkId, artists[i].DisplayName));
                i++;
            }

            var lookup = allArtworkArtists.ToLookup(kvp => kvp.Key, kvp => kvp.Value);

            var artworksOutput = await _repo.ExecuteReaderAsync<ArtworkOutputDto>("SELECT artwork_id, artwork.title, " +
                "artwork.description, primary_image_url, secondary_image_url, artwork.year_created, artwork.modified_at, " +
                "artwork.created_at, media_type, media.description as media_description, price FROM artwork INNER JOIN media ON " +
                "media.media_id = artwork.media_id");

            foreach (ArtworkOutputDto artwork in artworksOutput)
            {
                var artworkArtists = new List<String>();
                foreach (string artist in lookup[artwork.ArtworkId])
                {
                    artworkArtists.Add(artist);
                }

                artwork.ContributingArtists = artworkArtists;
            }

            return artworksOutput;
        }

        public async Task<ArtworkOutputDto?> GetArtworkByIdAsync(int id)
        {
            var artworkArtists = new List<String>();
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", id)
            };

            var artists = await _repo.ExecuteReaderAsync<ArtistOutputDto>("SELECT display_name FROM artist_artwork " +
                "INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id " +
                "WHERE artwork_id = @artworkId", sqlParams);

            var artworkOutputs = await _repo.ExecuteReaderAsync<ArtworkOutputDto>("SELECT artwork_id, artwork.title, " +
                "artwork.description, primary_image_url, secondary_image_url, year_created, artwork.modified_at, " +
                "artwork.created_at, media.media_type as media_type, price FROM artwork INNER JOIN media " +
                $"ON media.media_id = artwork.media_id WHERE artwork_id = {id}");

            var artworkOutput = artworkOutputs.SingleOrDefault();

            foreach (ArtistOutputDto artist in artists)
            {
                artworkArtists.Add(artist.DisplayName);
            }

            if (artworkOutput is not null) artworkOutput.ContributingArtists = artworkArtists;

            return artworkOutput;
        }

        public async Task<ArtworkInputDto?> InsertArtworkAsync(ArtworkInputDto artwork)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("title", textInfo.ToTitleCase(artwork.Title)),
                new("description", artwork.Description),
                new("mediaId", artwork.MediaId ?? (object)DBNull.Value),
                new("primary_image_url", artwork.PrimaryImageUrl),
                new("secondary_image_url", artwork.SecondaryImageUrl ?? (object)DBNull.Value),
                new("year_created", artwork.YearCreated ?? (object)DBNull.Value),
                new("price", artwork.Price)
            };

            var results = await _repo.ExecuteReaderAsync<ArtworkInputDto>("INSERT INTO artwork VALUES (DEFAULT, " +
                "@title, @description, @primary_image_url, @secondary_image_url, @year_created, @mediaId, " +
                "current_timestamp, current_timestamp, @price) RETURNING *", sqlParams);
                
            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<ArtworkInputDto?> UpdateArtworkAsync(int id, ArtworkInputDto artwork)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artwork_id", id),
                new("title", textInfo.ToTitleCase(artwork.Title)),
                new("description", artwork.Description),
                new("mediaId", artwork.MediaId ?? (object)DBNull.Value),
                new("primaryImageURL", artwork.PrimaryImageUrl),
                new("secondaryImageURL", artwork.SecondaryImageUrl ?? (object)DBNull.Value),
                new("yearCreated", artwork.YearCreated ?? (object)DBNull.Value),
                new("price", artwork.Price)
            };

            var results = await _repo.ExecuteReaderAsync<ArtworkInputDto>("UPDATE artwork SET title = @title, description = @description, " +
                "media_id = @mediaId, primary_image_url = @primaryImageURL, secondary_image_url = @secondaryImageURL, " +
                "year_created = @yearCreated, modified_at = current_timestamp, price = @price " +
                "WHERE artwork_id = @artwork_id RETURNING *", sqlParams);

            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<bool> DeleteArtworkAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", id)
            };

            var result = await _repo.ExecuteReaderAsync<ArtistOutputDto>("DELETE FROM artwork WHERE artwork_id = @artworkId", sqlParams);

            if (result is not null) return true;
            else return false;
        }

        public async Task<ArtistArtworkDto?> AllocateArtistAsync(int artistId, int artworkId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artist_id", artistId),
                new("artwork_id", artworkId)
            };

            var results = await _repo.ExecuteReaderAsync<ArtistArtworkDto>($"INSERT INTO artist_artwork(artist_id, artwork_id, " +
                "modified_at, created_at) VALUES (@artist_id, @artwork_id, current_timestamp, current_timestamp) " +
                "RETURNING *", sqlParams);

            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<bool> DeallocateArtistAsync(int artistId, int artworkId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", artistId),
                new("artworkId", artworkId)
            };

            var result = await _repo.ExecuteReaderAsync<ArtistArtworkDto>("DELETE FROM artist_artwork WHERE artist_id = @artistId " +
                "AND artwork_id = @artworkId", sqlParams);

            if (result is not null) return true;
            else return false;
        }
    }
}
