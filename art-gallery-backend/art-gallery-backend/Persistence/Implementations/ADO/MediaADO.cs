using Art_Gallery_Backend.Models.Database_Models;
using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence.Interfaces;
using Npgsql;

namespace Art_Gallery_Backend.Persistence.Implementations.ADO
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
            List<MediaOutputDto> mediaTypes = new();
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM media", connection);
                {
                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int mediaId = (int)dr["media_id"];
                                string mediaType = (string)dr["media_type"];
                                string description = (string)dr["description"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];
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
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM media WHERE media_id = @mediaId", connection);
                {
                    _ = cmd.Parameters.AddWithValue("@mediaId", id);
                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int mediaId = (int)dr["media_id"];
                                string mediaType = (string)dr["media_type"];
                                string description = (string)dr["description"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];
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
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("INSERT INTO media(media_type, description, modified_at, created_at) VALUES (@mediaType, @description, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@mediaType", media.MediaType);
                    cmd.Parameters.AddWithValue("@description", media.Description);
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? media : null;
                }
            }
        }
        // Option 1: The update function requires all DTO fields to be complete. Meaning database records can not be partially updated.
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

        // Option 2: The update function does not require all DTO fields to be complete. Any field left null, “”, or its default value; i.e., string, 0, ect, will not be updated. Nullable fields can not be set back to null after being updated.
/*        public MediaInputDto? UpdateMediaType(int mediaId, MediaInputDto media)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                string cmdString = "UPDATE media SET ";

                if (media.MediaType is not null and not "" and not "string")
                {
                    cmdString += "media_type = @mediaType, ";
                }
                if (media.Description is not null and not "" and not "string")
                {
                    cmdString += "description = @description, ";
                }

                cmdString += "modified_at = current_timestamp WHERE media_id = @mediaId";

                using NpgsqlCommand cmd = new(cmdString, connection);
                {
                    _ = cmd.Parameters.AddWithValue("@mediaId", mediaId);
                    if (media.MediaType is not null and not "" and not "string")
                    {
                        _ = cmd.Parameters.AddWithValue("@mediaType", media.MediaType);
                    }
                    if (media.Description is not null and not "" and not "string")
                    {
                        _ = cmd.Parameters.AddWithValue("@description", media.Description);
                    }
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? media : null;
                }
            }
        }*/

        public bool DeleteMediaType(int id)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("DELETE FROM media WHERE media_id = @mediaId", connection);
                {
                    cmd.Parameters.AddWithValue("@mediaId", id);
                    int result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}