using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ExhibitionRepository : IRepository, IExhibitionDataAccess
    {

        //TODO: Add data to database and test each of the following methods

        public ExhibitionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        private IRepository _repo => this;

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


        //TODO: Complete method

        public ExhibitionArtworkOutputDto? GetExhibitionArtworksById(int id)
        {
            throw new NotImplementedException();
        }



        public ExhibitionInputDto? InsertExhibition(ExhibitionInputDto exhibition)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("name", exhibition.Name),
                new("description", exhibition.Description),
                new("backgroundImageUrl", exhibition.BackgroundImageUrl)
            };

            var result = _repo.ExecuteReader<ExhibitionInputDto>("INSERT INTO exhibition(name, description, " +
                "background_image_url, modified_at, created_at) VALUES (@name, @description, @backgroundImageUrl, " +
                "current_timestamp, current_timestamp);", sqlParams)
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
                new("backgroundImageUrl", exhibition.BackgroundImageUrl)
            };

            var result = _repo.ExecuteReader<ExhibitionInputDto>("UPDATE exhibition SET name = @name, " +
                "description = @description, background_image_url = @backgroundImageUrl, " +
                "modified_at = current_timestamp WHERE exhibition_id = @exhibitionId", sqlParams)
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
                "current_timestamp, current_timestamp)", sqlParams)
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
