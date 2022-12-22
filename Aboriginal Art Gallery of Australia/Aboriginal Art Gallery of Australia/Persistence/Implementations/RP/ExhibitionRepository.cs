using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ExhibitionRepository : IRepository, IExhibitionDataAccess
    {

        //TODO: Test last three methods and ExhibitionArtworksById
        // These endpoints have all broken because the startdate and enddate are now DateOnly and that isn't mapping properly
        // I tried to fix in the MapTo extension method but no luck so far

        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public ExhibitionRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public List<ExhibitionOutputDto> GetExhibitions()
        {
            var exhibitions = _repo.ExecuteReader<ExhibitionOutputDto>("SELECT * FROM exhibition");
            return exhibitions;
        }

        public ExhibitionOutputDto? GetExhibitionById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("exhibitionId", id)
            };

            var exhibition = _repo.ExecuteReader<ExhibitionOutputDto>("SELECT * FROM exhibition " +
                "WHERE exhibition_id = @exhibitionId", sqlParams)
                .SingleOrDefault();

            return exhibition;
        }

        //TODO: Find a way to write lines 45 and 48 in a single SQL statement + check over logic
        public ExhibitionArtworkOutputDto? GetExhibitionArtworksById(int id)
        {
            var allArtworks = new List<ArtworkOutputDto>();
            var allArtworkArtists = new List<KeyValuePair<int, String>>();

            var artworks = _repo.ExecuteReader<ArtworkOutputDto>("SELECT artwork_id " +
                "FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id");

            var artists = _repo.ExecuteReader<ArtistOutputDto>("SELECT display_name FROM artist_artwork " +
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

            // This query is throwing InvalidOperation exception: 'the parameter already belongs to a collection'
            var artworksOutput = _repo.ExecuteReader<ArtworkOutputDto>("SELECT artwork_exhibition.artwork_id, " +
                "artwork.title, description, media, primary_image_url, secondary_image_url, year_created, " +
                "artwork.modified_at, artwork.created_at, nation.title as nation_title FROM artwork INNER JOIN " +
                "artwork_exhibition ON artwork_exhibition.artwork_id = artwork.artwork_id INNER JOIN nation ON " +
                "nation.nation_id = artwork.nation_id WHERE exhibition_id = @exhibitionId", sqlParams);

            foreach (ArtworkOutputDto artwork in artworksOutput)
            {
                var artworkArtists = new List<String>();
                foreach (string artist in lookup[artwork.ArtworkId])
                {
                    artworkArtists.Add(artist);
                }

                artwork.ContributingArtists = artworkArtists;
            }

            var exhibition = _repo.ExecuteReader<ExhibitionArtworkOutputDto>("SELECT * FROM exhibition " +
                "WHERE exhibition_id = @exhibitionId", sqlParams)
                .SingleOrDefault();

            if (exhibition is not null) exhibition.ExhibitionArtworks = artworksOutput;

            return exhibition;
        }

        public ExhibitionInputDto? InsertExhibition(ExhibitionInputDto exhibition)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("name", exhibition.Name),
                new("description", exhibition.Description),
                new("backgroundImageUrl", exhibition.BackgroundImageURL)
            };

            var result = _repo.ExecuteReader<ExhibitionInputDto>("INSERT INTO exhibition(name, description, " +
                "background_image_url, modified_at, created_at) VALUES (@name, @description, @backgroundImageUrl, " +
                "current_timestamp, current_timestamp) RETURNING *;", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public ExhibitionInputDto? UpdateExhibition(int id, ExhibitionInputDto exhibition)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("exhibitionId", id),
                new("name", exhibition.Name),
                new("description", exhibition.Description),
                new("backgroundImageUrl", exhibition.BackgroundImageURL)
            };

            var result = _repo.ExecuteReader<ExhibitionInputDto>("UPDATE exhibition SET name = @name, " +
                "description = @description, background_image_url = @backgroundImageUrl, " +
                "modified_at = current_timestamp WHERE exhibition_id = @exhibitionId RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public bool DeleteExhibition(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("exhibitionId", id)
            };

            _repo.ExecuteReader<ExhibitionOutputDto>("DELETE FROM exhibition WHERE " +
                "exhibition_id = @exhibitionId", sqlParams);

            return true;
        }

        public ArtworkExhibitionDto? AssignArtwork(int artworkId, int exhibitionId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", artworkId),
                new("exhibitionId", exhibitionId)
            };

            var result = _repo.ExecuteReader<ArtworkExhibitionDto>("INSERT INTO artwork_exhibition(artwork_id, " +
                "exhibition_id, modified_at, created_at) VALUES (@artworkId, @exhibitionId, " +
                "current_timestamp, current_timestamp) RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public bool DeassignArtwork(int artworkId, int exhibitionId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", artworkId),
                new("exhibitionId", exhibitionId)
            };

            _repo.ExecuteReader<ArtworkExhibitionDto>("DELETE FROM artwork_exhibition " +
                "WHERE artwork_id = @artworkId AND exhibition_id = @exhibitionId", sqlParams);

            return true;
        }
    }
}
