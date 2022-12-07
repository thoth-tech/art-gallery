using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO
{
    public class MediaADO : IMediaDataAccess
    {
        private readonly IConfiguration _configuration;

        public MediaADO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<MediaOutputDto> GetMediaTypes()
        {
            var mediaTypes = new List<MediaOutputDto>();
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM media", connection);
                {
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var mediaId = (int)dr["media_id"];
                                var mediaType = (string)dr["media_type"];
                                var description = (string)dr["description"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];
                                mediaTypes.Add(new MediaOutputDto(mediaId, mediaType, description, modifiedAt, createdAt));
                            }
                        }
                        return mediaTypes;
                    }
                }
            }
        }

        public MediaOutputDto? GetMediaTypeById(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM media WHERE media_id = @mediaId", connection);
                {
                    cmd.Parameters.AddWithValue("@mediaId", id);
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var mediaId = (int)dr["media_id"];
                                var mediaType = (string)dr["media_type"];
                                var description = (string)dr["description"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];
                                return new MediaOutputDto(mediaId, mediaType, description, modifiedAt, createdAt);
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public MediaInputDto? InsertMediaType(MediaInputDto media)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO media(media_type, description, modified_at, created_at) VALUES (@mediaType, @description, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@mediaType", media.MediaType);
                    cmd.Parameters.AddWithValue("@description", media.Description);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? media : null;
                }
            }
        }

        public MediaInputDto? UpdateMediaType(int mediaId, MediaInputDto media)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("UPDATE media SET media_type = @mediaType, description = @description, modified_at = current_timestamp WHERE media_id = @mediaId", connection);
                {
                    cmd.Parameters.AddWithValue("@mediaId", mediaId);
                    cmd.Parameters.AddWithValue("@mediaType", media.MediaType);
                    cmd.Parameters.AddWithValue("@description", media.Description);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? media : null;
                }
            }
        }

        public bool DeleteMediaType(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM media WHERE media_id = @mediaId", connection);
                {
                    cmd.Parameters.AddWithValue("@mediaId", id);
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}