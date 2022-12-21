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

        public List<ExhibitionOutputDto> GetExhibitions()
        {
            var exhibitions = new List<ExhibitionOutputDto>();
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM exhibition", connection);
                {
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var exhibitionId = (int)dr["exhibition_id"];
                                var name = (string)dr["name"];
                                var description = (string)dr["description"];
                                var backgroundImageUrl = (string)dr["background_image_url"];
                                var startDate = (DateTime)dr["start_date"];
                                var endDate = (DateTime)dr["end_date"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];

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
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM exhibition WHERE exhibition_id = @exhibitionId", connection);
                {
                    cmd.Parameters.AddWithValue("@exhibitionId", id);
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var exhibitionId = (int)dr["exhibition_id"];
                                var name = (string)dr["name"];
                                var description = (string)dr["description"];
                                var backgroundImageUrl = (string)dr["background_image_url"];
                                var startDate = (DateTime)dr["start_date"];
                                var endDate = (DateTime)dr["end_date"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];

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
            var artworks = new List<ArtworkOutputDto>();
            var allArtworkArtists = new List<KeyValuePair<int, String>>();
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd1 = new NpgsqlCommand("SELECT artwork_id, display_name as contributing_artist FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id", connection);
                {
                    using var dr = cmd1.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var artworkId = (int)dr["artwork_id"];
                                var contributingArtist = (string)dr["contributing_artist"];
                                allArtworkArtists.Add(new KeyValuePair<int, String>(artworkId, contributingArtist));
                            }
                        }
                    }
                }

                using var cmd2 = new NpgsqlCommand("SELECT artwork_exhibition.exhibition_id, artwork_exhibition.artwork_id, artwork.title, artwork.description, primary_image_url, secondary_image_url, year_created, artwork.modified_at, artwork.created_at, media.media_type " +
                                                   "FROM artwork " +
                                                   "INNER JOIN artwork_exhibition " +
                                                   "ON artwork_exhibition.artwork_id = artwork.artwork_id " +
                                                   "INNER JOIN media " +
                                                   "ON media.media_id = artwork.media_id " +
                                                   "WHERE exhibition_id = @ExhibitionId", connection);
                {
                    cmd2.Parameters.AddWithValue("@ExhibitionId", id);
                    using var dr = cmd2.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var artworkId = (int)dr["artwork_id"];
                                var title = (string)dr["title"];
                                var description = (string)dr["description"];
                                var primaryImageURL = (string)dr["primary_image_url"];
                                var secondaryImageURL = dr["secondary_image_url"] as string;
                                var createdYear = (int)dr["year_created"];
                                var mediaType = (string)dr["media_type"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];

                                var lookup = allArtworkArtists.ToLookup(kvp => kvp.Key, kvp => kvp.Value);
                                var artworkArtists = new List<String>();
                                foreach (string artist in lookup[artworkId])
                                    artworkArtists.Add(artist);

                                artworks.Add(new ArtworkOutputDto(artworkId, title, description, primaryImageURL, secondaryImageURL, createdYear, mediaType, modifiedAt, createdAt, artworkArtists));
                            }
                        }
                    }
                }

                using var cmd3 = new NpgsqlCommand("SELECT * FROM exhibition WHERE exhibition_id = @exhibitionId", connection);
                {
                    cmd3.Parameters.AddWithValue("@exhibitionId", id);
                    using var dr = cmd3.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var exhibitionId = (int)dr["exhibition_id"];
                                var name = (string)dr["name"];
                                var description = (string)dr["description"];
                                var backgroundImageUrl = (string)dr["background_image_url"];
                                var startDate = (DateTime)dr["start_date"];
                                var endDate = (DateTime)dr["end_date"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];

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
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO exhibition(name, description, background_image_url, start_date, end_date, modified_at, created_at) " +
                                                  "VALUES (@name, @description, @backgroundImageUrl, @startDate, @endDate, current_timestamp, current_timestamp);", connection);
                {
                    cmd.Parameters.AddWithValue("@name", exhibition.Name);
                    cmd.Parameters.AddWithValue("@description", exhibition.Description);
                    cmd.Parameters.AddWithValue("@backgroundImageUrl", exhibition.BackgroundImageURL);
                    cmd.Parameters.AddWithValue("@startDate", exhibition.StartDate);
                    cmd.Parameters.AddWithValue("@endDate", exhibition.EndDate);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? exhibition : null;
                }
            }
        }

        public ExhibitionInputDto? UpdateExhibition(int id, ExhibitionInputDto exhibition)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                String cmdString = "UPDATE exhibition SET ";

                if (exhibition.Name is not null && exhibition.Name != "" && exhibition.Name != "string")
                {
                    cmdString += "name = @name, ";
                }
                if (exhibition.Description is not null && exhibition.Description != "" && exhibition.Description != "string")
                {
                    cmdString += "description = @description, ";
                }
                if (exhibition.BackgroundImageURL is not null && exhibition.BackgroundImageURL != "" && exhibition.BackgroundImageURL != "string")
                {
                    cmdString += "background_image_url = @backgroundImageUrl, ";
                }
                if (exhibition.StartDate == default(DateOnly))
                {
                    cmdString += "start_date = @startDate, ";
                }
                if (exhibition.EndDate == default(DateOnly))
                {
                    cmdString += "end_date = @endDate, ";
                }

                cmdString += "modified_at = current_timestamp WHERE exhibition_id = @exhibitionId";

                using var cmd = new NpgsqlCommand(cmdString, connection);
                {
                    cmd.Parameters.AddWithValue("@exhibitionId", id);
                    if (exhibition.Name is not null && exhibition.Name != "" && exhibition.Name != "string")
                    {
                        cmd.Parameters.AddWithValue("@name", exhibition.Name);
                    }
                    if (exhibition.Description is not null && exhibition.Description != "" && exhibition.Description != "string")
                    {
                        cmd.Parameters.AddWithValue("@description", exhibition.Description);
                    }
                    if (exhibition.BackgroundImageURL is not null && exhibition.BackgroundImageURL != "" && exhibition.BackgroundImageURL != "string")
                    {
                        cmd.Parameters.AddWithValue("@backgroundImageUrl", exhibition.BackgroundImageURL);
                    }
                    if (exhibition.StartDate == default(DateOnly))
                    {
                        cmd.Parameters.AddWithValue("@startDate", exhibition.StartDate);
                    }
                    if (exhibition.EndDate == default(DateOnly))
                    {
                        cmd.Parameters.AddWithValue("@endDate", exhibition.EndDate);
                    }
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? exhibition : null;
                }
            }
        }

        public bool DeleteExhibition(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM exhibition WHERE exhibition_id = @exhibitionId", connection);
                {
                    cmd.Parameters.AddWithValue("@exhibitionId", id);
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }

        public ArtworkExhibitionDto? AssignArtwork(int artworkId, int exhibitionId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO artwork_exhibition(artwork_id, exhibition_id, modified_at, created_at) VALUES (@artworkId, @exhibitionId, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@artworkId", artworkId);
                    cmd.Parameters.AddWithValue("@exhibitionId", exhibitionId);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? new ArtworkExhibitionDto(artworkId, exhibitionId) : null;
                }
            }
        }

        public bool DeassignArtwork(int exhibitionId, int artworkId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM artwork_exhibition WHERE artwork_id = @artworkId AND exhibition_id = @exhibitionId ", connection);
                {
                    cmd.Parameters.AddWithValue("@artworkId", artworkId);
                    cmd.Parameters.AddWithValue("@exhibitionId", exhibitionId);
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}
