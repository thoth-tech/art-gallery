using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class MediaRepository : IRepository, IMediaDataAccess
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public MediaRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public List<MediaOutputDto> GetMediaTypes()
        {
            var mediaTypes = _repo.ExecuteReader<MediaOutputDto>("SELECT * FROM media");
            return mediaTypes;
        }

        public MediaOutputDto? GetMediaTypeById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("mediaId", id)
            };

            var media = _repo.ExecuteReader<MediaOutputDto>("SELECT * FROM media WHERE media_id = @mediaId", sqlParams)
                .SingleOrDefault();

            return media;
        }

        public MediaInputDto? InsertMediaType(MediaInputDto media)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("mediaType", media.MediaType),
                new("description", media.Description),
            };

            var result = _repo.ExecuteReader<MediaInputDto>("INSERT INTO media VALUES " +
                "(DEFAULT, @mediaType, @description, current_timestamp, current_timestamp) RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public MediaInputDto? UpdateMediaType(int id, MediaInputDto media)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("mediaId", id),
                new("mediaType", media.MediaType),
                new("description", media.Description),
            };

            var result = _repo.ExecuteReader<MediaInputDto>("UPDATE media SET media_type = @mediaType, description = @description, " +
                "modified_at = current_timestamp WHERE media_id = @mediaId RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public bool DeleteMediaType(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("mediaId", id)
            };

            _repo.ExecuteReader<MediaOutputDto>("DELETE FROM media WHERE media_id = @mediaId", sqlParams);

            return true;
        }
    }
}
