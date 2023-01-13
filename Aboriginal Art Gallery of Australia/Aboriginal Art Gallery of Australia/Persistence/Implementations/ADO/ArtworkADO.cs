using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO
{
    public class ArtworkADO : IArtworkDataAccess
    {
        private readonly IConfiguration _configuration;

        public ArtworkADO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<ArtworkOutputDto> GetArtworks()
        {
            var artworks = new List<ArtworkOutputDto>();
            var allArtworkArtists = new List<KeyValuePair<int, String>>();
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd1 = new NpgsqlCommand("SELECT artwork_id, display_name as contributing_artist " +
                                                   "FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id", connection);
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
                        connection.Close();
                    }
                }

                connection.Open();
                using var cmd2 = new NpgsqlCommand("SELECT artwork_id, artwork.title, artwork.description as artwork_description, primary_image_url, secondary_image_url, year_created, artwork.modified_at, artwork.created_at, media_type, media.description as media_description " +
                                                   "FROM artwork INNER JOIN media ON media.media_id = artwork.media_id", connection);
                {
                    using var dr = cmd2.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var artworkId = (int)dr["artwork_id"];
                                var title = (string)dr["title"];
                                var description = (string)dr["artwork_description"];
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
                        return artworks;
                    }
                }
            }
        }

        public ArtworkOutputDto? GetArtworkById(int id)
        {
            var artworkArtists = new List<String>();
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd1 = new NpgsqlCommand("SELECT artwork_id, display_name as contributing_artist " +
                                                   "FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id WHERE artwork_id = @artworkId", connection);
                {
                    cmd1.Parameters.AddWithValue("@artworkId", id);
                    using var dr = cmd1.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var contributingArtist = (string)dr["contributing_artist"];
                                artworkArtists.Add(contributingArtist);
                            }
                        }
                        connection.Close();
                    }
                }

                connection.Open();
                using var cmd2 = new NpgsqlCommand("SELECT artwork_id, artwork.title, artwork.description as artwork_description, primary_image_url, secondary_image_url, year_created, artwork.modified_at, artwork.created_at, media_type, media.description as media_description " +
                                                   "FROM artwork INNER JOIN media ON media.media_id = artwork.media_id where artwork_id = @artwork_id", connection);
                {
                    cmd2.Parameters.AddWithValue("@artwork_id", id);
                    using var dr = cmd2.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var artworkId = (int)dr["artwork_id"];
                                var title = (string)dr["title"];
                                var description = (string)dr["artwork_description"];
                                var primaryImageURL = (string)dr["primary_image_url"];
                                var secondaryImageURL = dr["secondary_image_url"] as string;
                                var createdYear = (int)dr["year_created"];
                                var mediaType = (string)dr["media_type"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];

                                return new ArtworkOutputDto(artworkId, title, description, primaryImageURL, secondaryImageURL, createdYear, mediaType, modifiedAt, createdAt, artworkArtists);
                            }
                        }
                        return null;
                    }
                }
            }
        }


        public ArtworkOutputDto? GetArtworkOfTheDay()
        {
            // Ultra hacky to just get something working for you guys. Will swap between artwork 1 and artwork 2 each minute.
            if (DateTime.Now.Minute % 2 == 0)
                return GetArtworkById(1);
            else return GetArtworkById(2);
        }

        public ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO artwork(title, description, primary_image_url, secondary_image_url, year_created, media_id, modified_at, created_at) " +
                                                  "VALUES (@title, @description, @primaryImageURL, @secondaryImageURL, @yearCreated, @mediaId, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@title", artwork.Title);
                    cmd.Parameters.AddWithValue("@description", artwork.Description);
                    cmd.Parameters.AddWithValue("@primaryImageURL", artwork.PrimaryImageUrl);
                    cmd.Parameters.AddWithNullableValue("@secondaryImageURL", artwork.SecondaryImageUrl);
                    cmd.Parameters.AddWithNullableValue("@yearCreated", artwork.YearCreated);
                    cmd.Parameters.AddWithNullableValue("@mediaId", artwork.MediaId);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? artwork : null;
                }
            }
        }

        public ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                String cmdString = "UPDATE artwork SET ";

                if (artwork.Title is not null && artwork.Title != "" && artwork.Title != "string")
                {
                    cmdString += "title = @title, ";
                }
                if (artwork.Description is not null && artwork.Description != "" && artwork.Description != "string")
                {
                    cmdString += "description = @description, ";
                }
                if (artwork.PrimaryImageUrl is not null && artwork.PrimaryImageUrl != "" && artwork.PrimaryImageUrl != "string")
                {
                    cmdString += "primary_image_url = @primaryImageURL, ";
                }
                if (artwork.SecondaryImageUrl is not null && artwork.SecondaryImageUrl != "" && artwork.SecondaryImageUrl != "string")
                {
                    cmdString += "secondary_image_url = @secondaryImageURL, ";
                }
                if (artwork.YearCreated is not null && artwork.YearCreated != 0)
                {
                    cmdString += "year_created = @yearCreated, ";
                }
                if (artwork.MediaId is not null && artwork.MediaId != 0)
                {
                    cmdString += "media_id = @mediaId, ";
                }

                cmdString += "modified_at = current_timestamp WHERE artwork_id = @artwork_id";

                using var cmd = new NpgsqlCommand(cmdString, connection);
                {
                    cmd.Parameters.AddWithValue("@artwork_id", id);
                    if (artwork.Title is not null && artwork.Title != "" && artwork.Title != "string")
                    {
                        cmd.Parameters.AddWithValue("@title", artwork.Title);
                    }
                    if (artwork.Description is not null && artwork.Description != "" && artwork.Description != "string")
                    {
                        cmd.Parameters.AddWithValue("@description", artwork.Description);
                    }
                    if (artwork.PrimaryImageUrl is not null && artwork.PrimaryImageUrl != "" && artwork.PrimaryImageUrl != "string")
                    {
                        cmd.Parameters.AddWithValue("@primaryImageURL", artwork.PrimaryImageUrl);
                    }
                    if (artwork.SecondaryImageUrl is not null && artwork.SecondaryImageUrl != "" && artwork.SecondaryImageUrl != "string")
                    {
                        cmd.Parameters.AddWithNullableValue("@secondaryImageURL", artwork.SecondaryImageUrl);
                    }
                    if (artwork.YearCreated is not null && artwork.YearCreated != 0)
                    {
                        cmd.Parameters.AddWithNullableValue("@yearCreated", artwork.YearCreated);
                    }
                    if (artwork.MediaId is not null && artwork.MediaId != 0)
                    {
                        cmd.Parameters.AddWithNullableValue("@mediaId", artwork.MediaId);
                    }
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? artwork : null;
                }
            }
        }

        public bool DeleteArtwork(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM artwork WHERE artwork_id = @artworkId", connection);
                {
                    cmd.Parameters.AddWithValue("@artworkId", id);
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }

        public ArtistArtworkDto? AssignArtist(int artistId, int artworkId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO artist_artwork(artist_id, artwork_id, modified_at, created_at) VALUES (@artistId, @artworkId, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", artistId);
                    cmd.Parameters.AddWithValue("@artworkId", artworkId);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? new ArtistArtworkDto(artistId, artworkId) : null;
                }
            }
        }

        public bool DeassignArtist(int artistId, int artworkId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                using var cmd = new NpgsqlCommand("DELETE FROM artist_artwork WHERE artist_id = @artistId AND artwork_id = @artworkId", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", artistId);
                    cmd.Parameters.AddWithValue("@artworkId", artworkId);
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }

    }
}