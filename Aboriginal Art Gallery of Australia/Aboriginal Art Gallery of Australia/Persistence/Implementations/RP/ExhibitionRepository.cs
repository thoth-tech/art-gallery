using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ExhibitionRepository : IRepository, IExhibitionDataAccess
    {
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

            var artworksOutput = _repo.ExecuteReader<ArtworkOutputDto>("SELECT artwork_exhibition.artwork_id, " +
                "artwork.title, artwork.description, primary_image_url, secondary_image_url, year_created, " +
                "artwork.modified_at, artwork.created_at, media.media_type as media_type FROM artwork INNER JOIN " +
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

            var exhibition = _repo.ExecuteReader<ExhibitionArtworkOutputDto>("SELECT * FROM exhibition " +
                $"WHERE exhibition_id = {id}")
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
                new("backgroundImageUrl", exhibition.BackgroundImageUrl),
                new("startdate", exhibition.StartDate),
                new("enddate", exhibition.EndDate)
            };

            var result = _repo.ExecuteReader<ExhibitionInputDto>("INSERT INTO exhibition(name, description, " +
                "background_image_url, start_date, end_date, modified_at, created_at) VALUES (@name, @description, " +
                "@backgroundImageUrl, @startdate, @enddate, current_timestamp, current_timestamp) RETURNING *;", sqlParams)
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
                new("backgroundImageUrl", exhibition.BackgroundImageUrl),
                new("startdate", exhibition.StartDate),
                new("enddate", exhibition.EndDate)
            };

            var result = _repo.ExecuteReader<ExhibitionInputDto>("UPDATE exhibition SET name = @name, " +
                "description = @description, background_image_url = @backgroundImageUrl, start_date = @startdate, " +
                "end_date=@enddate, modified_at = current_timestamp WHERE exhibition_id = @exhibitionId RETURNING *", sqlParams)
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

        public ArtworkExhibitionDto? AllocateArtwork(int artworkId, int exhibitionId)
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

        public bool DeallocateArtwork(int artworkId, int exhibitionId)
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
