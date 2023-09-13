using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence.Interfaces;
using static Art_Gallery_Backend.Persistence.ExtensionMethods;
using Npgsql;

namespace Art_Gallery_Backend.Persistence.Implementations.RP
{
    public class MediaRepository : IRepository, IMediaDataAccessAsync
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public MediaRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<MediaOutputDto>> GetMediaTypesAsync()
        {
            var mediatypes = await _repo.ExecuteReaderAsync<MediaOutputDto>("SELECT * FROM media");
            return mediatypes;
        }

        public async Task<MediaOutputDto?> GetMediaTypeByIdAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("mediaId", id)
            };

            var mediatypes = await _repo.ExecuteReaderAsync<MediaOutputDto>("SELECT * FROM media WHERE media_id = @mediaId", sqlParams);
            var media = mediatypes.SingleOrDefault();

            return media;
        }

        public async Task<MediaInputDto?> InsertMediaTypeAsync(MediaInputDto media)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("mediaType", media.MediaType),
                new("description", media.Description)
            };

            var results = await _repo.ExecuteReaderAsync<MediaInputDto>("INSERT INTO media(media_type, description, modified_at, created_at) VALUES (@mediaType, @description, current_timestamp, current_timestamp) RETURNING *", sqlParams);
            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<MediaInputDto?> UpdateMediaTypeAsync(int id, MediaInputDto media)
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

            var results = await _repo.ExecuteReaderAsync<MediaInputDto>(cmdString, sqlParams);
            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<bool> DeleteMediaTypeAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("mediaId", id)
            };

            var result = await _repo.ExecuteReaderAsync<MediaOutputDto>("DELETE FROM media WHERE media_id = @mediaId", sqlParams);

            if (result is not null) return true;
            else return false;
        }
    }
}
