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
            var mediatypes = _repo.ExecuteReader<MediaOutputDto>("SELECT * FROM media");
            return mediatypes;
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
                new("description", media.Description)
            };

            var result = _repo.ExecuteReader<MediaInputDto>("INSERT INTO media(media_type, description, modified_at, created_at) VALUES (@mediaType, @description, current_timestamp, current_timestamp) RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public MediaInputDto? UpdateMediaType(int id, MediaInputDto media)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("mediaId", id),
                new("mediaType", media.MediaType),
                new("description", media.Description)
            };

            // Allowing update of only one field
            String cmdString = "UPDATE media SET ";

            if (media.MediaType is not null && media.MediaType != "" && media.MediaType != "string")
            {
                cmdString += "media_type = @mediaType, ";
            }
            if (media.Description is not null && media.Description != "" && media.Description != "string")
            {
                cmdString += "description = @description, ";
            }

            cmdString += "modified_at = current_timestamp WHERE media_id = @mediaId RETURNING *";

            var result = _repo.ExecuteReader<MediaInputDto>(cmdString, sqlParams).SingleOrDefault();

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
