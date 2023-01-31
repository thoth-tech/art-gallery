using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    /// <summary>
    /// The Media class is responsible for handling the database model associated with various media types. 
    /// </summary>
    public class Media : IMediaDataAccess
    {
        public int MediaId { get; set; }
        public string MediaType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Media(int mediaId, string mediaType, string description, DateTime modifiedAt, DateTime createdAt)
        {
            MediaId = mediaId;
            MediaType = mediaType;
            Description = description;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 25 is required for the active record implementation.
        private static string _connectionString = "Host=localhost;Database=Deakin University | AAGoA;Username=postgres;Password=postgreSQL;";

        public Media() { }

        public List<MediaOutputDto> GetMediaTypes()
        {
            List<MediaOutputDto> mediaTypes = new();
            using NpgsqlConnection connection = new(_connectionString);
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
            using NpgsqlConnection connection = new(_connectionString);
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
            using NpgsqlConnection connection = new(_connectionString);
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

        public MediaInputDto? UpdateMediaType(int mediaId, MediaInputDto media)
        {
            using var connection = new NpgsqlConnection(_connectionString);
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
            using NpgsqlConnection connection = new(_connectionString);
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