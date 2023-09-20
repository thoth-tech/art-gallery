using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence.Interfaces;
using static Art_Gallery_Backend.Persistence.ExtensionMethods;
using Npgsql;
using System.Globalization;
using Art_Gallery_Backend.Models.Database_Models;

namespace Art_Gallery_Backend.Persistence.Implementations.RP
{
    public class ExhibitionRepository : IRepository, IExhibitionDataAccessAsync
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public ExhibitionRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        public async Task<List<ExhibitionOutputDto>> GetExhibitionsAsync()
        {
            var exhibitions = await _repo.ExecuteReaderAsync<ExhibitionOutputDto>("SELECT * FROM exhibition");
            return exhibitions;
        }

        public async Task<ExhibitionOutputDto?> GetExhibitionByIdAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("exhibitionId", id)
            };

            var exhibitions = await _repo.ExecuteReaderAsync<ExhibitionOutputDto>("SELECT * FROM exhibition " +
                "WHERE exhibition_id = @exhibitionId", sqlParams);
                
            var exhibition = exhibitions.SingleOrDefault();

            return exhibition;
        }

        public async Task<ExhibitionArtworkOutputDto?> GetExhibitionArtworksByIdAsync(int id)
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

            var sqlParams = new NpgsqlParameter[]
            {
                new("exhibitionId", id)
            };

            var artworksOutput = await _repo.ExecuteReaderAsync<ArtworkOutputDto>("SELECT artwork_exhibition.artwork_id, " +
                "artwork.title, artwork.description, primary_image_url, secondary_image_url, year_created, " +
                "artwork.modified_at, artwork.created_at, media.media_type as media_type, price FROM artwork INNER JOIN " +
                "artwork_exhibition ON artwork_exhibition.artwork_id = artwork.artwork_id INNER JOIN media ON " +
                "media.media_id = artwork.media_id WHERE exhibition_id = @exhibitionId", sqlParams);

            foreach (ArtworkOutputDto artwork in artworksOutput)
            {
                var artworkArtists = new List<String>();
                foreach (string artist in lookup[artwork.ArtworkId])
                {
                    artworkArtists.Add(artist);
                }

                artwork.ContributingArtists = artworkArtists;
            }

            var exhibitions = await _repo.ExecuteReaderAsync<ExhibitionArtworkOutputDto>("SELECT * FROM exhibition " +
                $"WHERE exhibition_id = {id}");
                
            var exhibition = exhibitions.SingleOrDefault();

            if (exhibition is not null) exhibition.ExhibitionArtworks = artworksOutput;

            return exhibition;
        }

        public async Task<ExhibitionInputDto?> InsertExhibitionAsync(ExhibitionInputDto exhibition)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("name", textInfo.ToTitleCase(exhibition.Name)),
                new("description", exhibition.Description),
                new("backgroundImageUrl", exhibition.BackgroundImageUrl),
                new("startdate", exhibition.StartDate),
                new("enddate", exhibition.EndDate)
            };

            var results = await _repo.ExecuteReaderAsync<ExhibitionInputDto>("INSERT INTO exhibition(name, description, " +
                "background_image_url, start_date, end_date, modified_at, created_at) VALUES (@name, @description, " +
                "@backgroundImageUrl, @startdate, @enddate, current_timestamp, current_timestamp) RETURNING *;", sqlParams);

            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<ExhibitionInputDto?> UpdateExhibitionAsync(int id, ExhibitionInputDto exhibition)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("exhibitionId", id),
                new("name", textInfo.ToTitleCase(exhibition.Name)),
                new("description", exhibition.Description),
                new("backgroundImageUrl", exhibition.BackgroundImageUrl),
                new("startdate", exhibition.StartDate),
                new("enddate", exhibition.EndDate)
            };

            var results = await _repo.ExecuteReaderAsync<ExhibitionInputDto>("UPDATE exhibition SET name = @name, " +
                "description = @description, background_image_url = @backgroundImageUrl, start_date = @startdate, " +
                "end_date=@enddate, modified_at = current_timestamp WHERE exhibition_id = @exhibitionId RETURNING *", sqlParams);
           
            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<bool> DeleteExhibitionAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("exhibitionId", id)
            };

            var result = await _repo.ExecuteReaderAsync<ExhibitionOutputDto>("DELETE FROM exhibition WHERE " +
                "exhibition_id = @exhibitionId", sqlParams);

            if (result is not null) return true;
            else return false;
        }

        public async Task<ArtworkExhibitionDto?> AllocateArtworkAsync(int artworkId, int exhibitionId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", artworkId),
                new("exhibitionId", exhibitionId)
            };

            var results = await _repo.ExecuteReaderAsync<ArtworkExhibitionDto>("INSERT INTO artwork_exhibition(artwork_id, " +
                "exhibition_id, modified_at, created_at) VALUES (@artworkId, @exhibitionId, " +
                "current_timestamp, current_timestamp) RETURNING *", sqlParams);
            
            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<bool> DeallocateArtworkAsync(int artworkId, int exhibitionId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", artworkId),
                new("exhibitionId", exhibitionId)
            };

            var result = await _repo.ExecuteReaderAsync<ArtworkExhibitionDto>("DELETE FROM artwork_exhibition " +
                "WHERE artwork_id = @artworkId AND exhibition_id = @exhibitionId", sqlParams);

            if (result is not null) return true;
            else return false;
        }
    }
}
