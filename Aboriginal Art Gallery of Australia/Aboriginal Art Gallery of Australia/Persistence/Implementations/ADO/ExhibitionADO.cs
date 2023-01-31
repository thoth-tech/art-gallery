using System.Globalization;
using Aboriginal_Art_Gallery_of_Australia.Models.Database_Models;
using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO
{
    public class ExhibitionADO : IExhibitionDataAccess
    {
        private readonly IConfiguration _configuration;

        public ExhibitionADO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        public List<ExhibitionOutputDto> GetExhibitions()
        {
            List<ExhibitionOutputDto> exhibitions = new();
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM exhibition", connection);
                {
                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int exhibitionId = (int)dr["exhibition_id"];
                                string name = (string)dr["name"];
                                string description = (string)dr["description"];
                                string backgroundImageUrl = (string)dr["background_image_url"];
                                DateTime startDate = (DateTime)dr["start_date"];
                                DateTime endDate = (DateTime)dr["end_date"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];

                                exhibitions.Add(new ExhibitionOutputDto(exhibitionId, name, description, backgroundImageUrl, DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate), modifiedAt, createdAt));
                            }
                        }
                        return exhibitions;
                    }
                }
            }
        }

        public ExhibitionOutputDto? GetExhibitionById(int id)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM exhibition WHERE exhibition_id = @exhibitionId", connection);
                {
                    _ = cmd.Parameters.AddWithValue("@exhibitionId", id);
                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int exhibitionId = (int)dr["exhibition_id"];
                                string name = (string)dr["name"];
                                string description = (string)dr["description"];
                                string backgroundImageUrl = (string)dr["background_image_url"];
                                DateTime startDate = (DateTime)dr["start_date"];
                                DateTime endDate = (DateTime)dr["end_date"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];

                                return new ExhibitionOutputDto(exhibitionId, name, description, backgroundImageUrl, DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate), modifiedAt, createdAt);
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public ExhibitionArtworkOutputDto? GetExhibitionArtworksById(int id)
        {
            List<ArtworkOutputDto> artworks = new();
            List<KeyValuePair<int, string>> allArtworkArtists = new();
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd1 = new("SELECT artwork_id, display_name as contributing_artist FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id", connection);
                {
                    using NpgsqlDataReader dr = cmd1.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int artworkId = (int)dr["artwork_id"];
                                string contributingArtist = (string)dr["contributing_artist"];
                                allArtworkArtists.Add(new KeyValuePair<int, string>(artworkId, contributingArtist));
                            }
                        }
                    }
                }

                using NpgsqlCommand cmd2 = new("SELECT artwork_exhibition.exhibition_id, artwork_exhibition.artwork_id, artwork.title, artwork.description, primary_image_url, secondary_image_url, year_created, artwork.modified_at, artwork.created_at, media.media_type " +
                                                   "FROM artwork " +
                                                   "INNER JOIN artwork_exhibition " +
                                                   "ON artwork_exhibition.artwork_id = artwork.artwork_id " +
                                                   "INNER JOIN media " +
                                                   "ON media.media_id = artwork.media_id " +
                                                   "WHERE exhibition_id = @ExhibitionId", connection);
                {
                    cmd2.Parameters.AddWithValue("@ExhibitionId", id);
                    using NpgsqlDataReader dr = cmd2.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int artworkId = (int)dr["artwork_id"];
                                string title = (string)dr["title"];
                                string description = (string)dr["description"];
                                string primaryImageURL = (string)dr["primary_image_url"];
                                string? secondaryImageURL = dr["secondary_image_url"] as string;
                                int createdYear = (int)dr["year_created"];
                                string mediaType = (string)dr["media_type"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];

                                ILookup<int, string> lookup = allArtworkArtists.ToLookup(kvp => kvp.Key, kvp => kvp.Value);
                                List<string> artworkArtists = new();
                                foreach (string artist in lookup[artworkId])
                                {
                                    artworkArtists.Add(artist);
                                }

                                artworks.Add(new ArtworkOutputDto(artworkId, title, description, primaryImageURL, secondaryImageURL, createdYear, mediaType, modifiedAt, createdAt, artworkArtists));
                            }
                        }
                    }
                }

                using NpgsqlCommand cmd3 = new("SELECT * FROM exhibition WHERE exhibition_id = @exhibitionId", connection);
                {
                    cmd3.Parameters.AddWithValue("@exhibitionId", id);
                    using NpgsqlDataReader dr = cmd3.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int exhibitionId = (int)dr["exhibition_id"];
                                string name = (string)dr["name"];
                                string description = (string)dr["description"];
                                string backgroundImageUrl = (string)dr["background_image_url"];
                                DateTime startDate = (DateTime)dr["start_date"];
                                DateTime endDate = (DateTime)dr["end_date"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];

                                return new ExhibitionArtworkOutputDto(exhibitionId, name, description, backgroundImageUrl, DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate), modifiedAt, createdAt, artworks);
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public ExhibitionInputDto? InsertExhibition(ExhibitionInputDto exhibition)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("INSERT INTO exhibition(name, description, background_image_url, start_date, end_date, modified_at, created_at) " +
                                                  "VALUES (@name, @description, @backgroundImageUrl, @startDate, @endDate, current_timestamp, current_timestamp);", connection);
                {
                    cmd.Parameters.AddWithValue("@name", textInfo.ToTitleCase(exhibition.Name));
                    cmd.Parameters.AddWithValue("@description", exhibition.Description);
                    cmd.Parameters.AddWithValue("@backgroundImageUrl", exhibition.BackgroundImageUrl);
                    cmd.Parameters.AddWithValue("@startDate", exhibition.StartDate);
                    cmd.Parameters.AddWithValue("@endDate", exhibition.EndDate);
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? exhibition : null;
                }
            }
        }

        // Option 1: The update function requires all DTO fields to be complete. Meaning database records can not be partially updated.
        public ExhibitionInputDto? UpdateExhibition(int id, ExhibitionInputDto exhibition)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("UPDATE exhibition " +
                                              "SET name = @name, " +
                                                  "description = @description, " +
                                                  "background_image_url = @backgroundImageUrl, " +
                                                  "modified_at = current_timestamp " +
                                              "WHERE exhibition_id = @exhibitionId", connection);
                {
                    cmd.Parameters.AddWithValue("@exhibitionId", id);
                    cmd.Parameters.AddWithValue("@name", textInfo.ToTitleCase(exhibition.Name));
                    cmd.Parameters.AddWithValue("@description", exhibition.Description);
                    cmd.Parameters.AddWithNullableValue("@backgroundImageUrl", exhibition.BackgroundImageUrl);
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? exhibition : null;
                }
            }
        }

        // Option 2: The update function does not require all DTO fields to be complete. Any field left null, “”, or its default value; i.e., string, 0, ect, will not be updated. Nullable fields can not be set back to null after being updated.
        /*public ExhibitionInputDto? UpdateExhibition(int id, ExhibitionInputDto exhibition)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                string cmdString = "UPDATE exhibition SET ";

                if (exhibition.Name is not null and not "" and not "string")
                {
                    cmdString += "name = @name, ";
                }
                if (exhibition.Description is not null and not "" and not "string")
                {
                    cmdString += "description = @description, ";
                }
                if (exhibition.BackgroundImageUrl is not null and not "" and not "string")
                {
                    cmdString += "background_image_url = @backgroundImageUrl, ";
                }
                if (exhibition.StartDate != default)
                {
                    cmdString += "start_date = @startDate, ";
                }
                if (exhibition.EndDate != default)
                {
                    cmdString += "end_date = @endDate, ";
                }

                cmdString += "modified_at = current_timestamp WHERE exhibition_id = @exhibitionId";

                using NpgsqlCommand cmd = new(cmdString, connection);
                {
                    _ = cmd.Parameters.AddWithValue("@exhibitionId", id);
                    if (exhibition.Name is not null and not "" and not "string")
                    {
                        _ = cmd.Parameters.AddWithValue("@name", textInfo.ToTitleCase(exhibition.Name));
                    }
                    if (exhibition.Description is not null and not "" and not "string")
                    {
                        _ = cmd.Parameters.AddWithValue("@description", exhibition.Description);
                    }
                    if (exhibition.BackgroundImageUrl is not null and not "" and not "string")
                    {
                        _ = cmd.Parameters.AddWithValue("@backgroundImageUrl", exhibition.BackgroundImageUrl);
                    }
                    if (exhibition.StartDate != default)
                    {
                        _ = cmd.Parameters.AddWithValue("@startDate", exhibition.StartDate);
                    }
                    if (exhibition.EndDate != default)
                    {
                        _ = cmd.Parameters.AddWithValue("@endDate", exhibition.EndDate);
                    }
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? exhibition : null;
                }
            }
        }*/

        public bool DeleteExhibition(int id)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("DELETE FROM exhibition WHERE exhibition_id = @exhibitionId", connection);
                {
                    cmd.Parameters.AddWithValue("@exhibitionId", id);
                    int result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }

        public ArtworkExhibitionDto? AllocateArtwork(int artworkId, int exhibitionId)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("INSERT INTO artwork_exhibition(artwork_id, exhibition_id, modified_at, created_at) VALUES (@artworkId, @exhibitionId, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@artworkId", artworkId);
                    cmd.Parameters.AddWithValue("@exhibitionId", exhibitionId);
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? new ArtworkExhibitionDto(artworkId, exhibitionId) : null;
                }
            }
        }

        public bool DeallocateArtwork(int exhibitionId, int artworkId)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("DELETE FROM artwork_exhibition WHERE artwork_id = @artworkId AND exhibition_id = @exhibitionId ", connection);
                {
                    cmd.Parameters.AddWithValue("@artworkId", artworkId);
                    cmd.Parameters.AddWithValue("@exhibitionId", exhibitionId);
                    int result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}
