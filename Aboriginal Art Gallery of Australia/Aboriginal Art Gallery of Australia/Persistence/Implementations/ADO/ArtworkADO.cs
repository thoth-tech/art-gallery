using System.Globalization;
using Aboriginal_Art_Gallery_of_Australia.Models.Database_Models;
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

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        public List<ArtworkOutputDto> GetArtworks()
        {
            List<ArtworkOutputDto> artworks = new();
            List<KeyValuePair<int, string>> allArtworkArtists = new();
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd1 = new("SELECT artwork_id, display_name as contributing_artist " +
                                               "FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id", connection);
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
                        connection.Close();
                    }
                }

                connection.Open();
                using NpgsqlCommand cmd2 = new("SELECT artwork_id, artwork.title, artwork.description as artwork_description, primary_image_url, secondary_image_url, year_created, artwork.modified_at, artwork.created_at, media_type, media.description as media_description " +
                                               "FROM artwork INNER JOIN media ON media.media_id = artwork.media_id", connection);
                {
                    using NpgsqlDataReader dr = cmd2.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int artworkId = (int)dr["artwork_id"];
                                string title = (string)dr["title"];
                                string description = (string)dr["artwork_description"];
                                string primaryImageURL = (string)dr["primary_image_url"];
                                string? secondaryImageURL = (string)dr["secondary_image_url"];
                                int? createdYear = (int)dr["year_created"];
                                string? mediaType = (string)dr["media_type"];
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
                        return artworks;
                    }
                }
            }
        }

        public ArtworkOutputDto? GetArtworkById(int id)
        {
            List<string> artworkArtists = new();
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd1 = new("SELECT artwork_id, display_name as contributing_artist " +
                                               "FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id WHERE artwork_id = @artworkId", connection);
                {
                    cmd1.Parameters.AddWithValue("@artworkId", id);
                    using NpgsqlDataReader dr = cmd1.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                string contributingArtist = (string)dr["contributing_artist"];
                                artworkArtists.Add(contributingArtist);
                            }
                        }
                        connection.Close();
                    }
                }

                connection.Open();
                using NpgsqlCommand cmd2 = new("SELECT artwork_id, artwork.title, artwork.description as artwork_description, primary_image_url, secondary_image_url, year_created, artwork.modified_at, artwork.created_at, media_type, media.description as media_description " +
                                               "FROM artwork INNER JOIN media ON media.media_id = artwork.media_id where artwork_id = @artwork_id", connection);
                {
                    cmd2.Parameters.AddWithValue("@artwork_id", id);
                    using NpgsqlDataReader dr = cmd2.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int artworkId = (int)dr["artwork_id"];
                                string title = (string)dr["title"];
                                string description = (string)dr["artwork_description"];
                                string primaryImageURL = (string)dr["primary_image_url"];
                                string? secondaryImageURL = (string)dr["secondary_image_url"];
                                int? createdYear = (int)dr["year_created"];
                                string? mediaType = (string)dr["media_type"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];

                                return new ArtworkOutputDto(artworkId, title, description, primaryImageURL, secondaryImageURL, createdYear, mediaType, modifiedAt, createdAt, artworkArtists);
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("INSERT INTO artwork(title, description, primary_image_url, secondary_image_url, year_created, media_id, modified_at, created_at) " +
                                              "VALUES (@title, @description, @primaryImageURL, @secondaryImageURL, @yearCreated, @mediaId, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@title", textInfo.ToTitleCase(artwork.Title));
                    cmd.Parameters.AddWithValue("@description", artwork.Description);
                    cmd.Parameters.AddWithValue("@primaryImageURL", artwork.PrimaryImageUrl);
                    cmd.Parameters.AddWithNullableValue("@secondaryImageURL", artwork.SecondaryImageUrl);
                    cmd.Parameters.AddWithNullableValue("@yearCreated", artwork.YearCreated);
                    cmd.Parameters.AddWithNullableValue("@mediaId", artwork.MediaId);
                    int result = cmd.ExecuteNonQuery();

                    return result is 1 ? artwork : null;
                }
            }
        }

        // Option 1: The update function requires all DTO fields to be complete. Meaning database records can not be partially updated.
        public ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("UPDATE artwork " +
                                                  "SET title = @title, " +
                                                       "description = @description, " +
                                                       "media = @media, " +
                                                       "primary_image_url = @primaryImageURL, " +
                                                       "secondary_image_url = @secondaryImageURL, " +
                                                       "year_created = @yearCreated, " +
                                                       "media_id = @mediaId, " +
                                                       "modified_at=current_timestamp " +
                                                  "WHERE artwork_id = @artwork_id", connection);
                {
                    cmd.Parameters.AddWithValue("@artwork_id", id);
                    cmd.Parameters.AddWithValue("@title", textInfo.ToTitleCase(artwork.Title));
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

        // Option 2: The update function does not require all DTO fields to be complete. Any field left null, “”, or its default value; i.e., string, 0, ect, will not be updated. Nullable fields can not be set back to null after being updated.
        /*        public ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork)
                {
                    using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
                    {
                        connection.Open();

                        string cmdString = "UPDATE artwork SET ";

                        if (artwork.Title is not null and not "" and not "string")
                        {
                            cmdString += "title = @title, ";
                        }
                        if (artwork.Description is not null and not "" and not "string")
                        {
                            cmdString += "description = @description, ";
                        }
                        if (artwork.PrimaryImageUrl is not null and not "" and not "string")
                        {
                            cmdString += "primary_image_url = @primaryImageURL, ";
                        }
                        if (artwork.SecondaryImageUrl is not null and not "" and not "string")
                        {
                            cmdString += "secondary_image_url = @secondaryImageURL, ";
                        }
                        if (artwork.YearCreated is not null and not 0)
                        {
                            cmdString += "year_created = @yearCreated, ";
                        }
                        if (artwork.MediaId is not null and not 0)
                        {
                            cmdString += "media_id = @mediaId, ";
                        }

                        cmdString += "modified_at = current_timestamp WHERE artwork_id = @artwork_id";

                        using NpgsqlCommand cmd = new(cmdString, connection);
                        {
                            _ = cmd.Parameters.AddWithValue("@artwork_id", id);
                            if (artwork.Title is not null and not "" and not "string")
                            {
                                _ = cmd.Parameters.AddWithValue("@title", textInfo.ToTitleCase(artwork.Title));
                            }
                            if (artwork.Description is not null and not "" and not "string")
                            {
                                _ = cmd.Parameters.AddWithValue("@description", artwork.Description);
                            }
                            if (artwork.PrimaryImageUrl is not null and not "" and not "string")
                            {
                                _ = cmd.Parameters.AddWithValue("@primaryImageURL", artwork.PrimaryImageUrl);
                            }
                            if (artwork.SecondaryImageUrl is not null and not "" and not "string")
                            {
                                _ = cmd.Parameters.AddWithNullableValue("@secondaryImageURL", artwork.SecondaryImageUrl);
                            }
                            if (artwork.YearCreated is not null and not 0)
                            {
                                _ = cmd.Parameters.AddWithNullableValue("@yearCreated", artwork.YearCreated);
                            }
                            if (artwork.MediaId is not null and not 0)
                            {
                                _ = cmd.Parameters.AddWithNullableValue("@mediaId", artwork.MediaId);
                            }
                            int result = cmd.ExecuteNonQuery();
                            return result is 1 ? artwork : null;
                        }
                    }
                }*/

        public bool DeleteArtwork(int id)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("DELETE FROM artwork WHERE artwork_id = @artworkId", connection);
                {
                    cmd.Parameters.AddWithValue("@artworkId", id);
                    int result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }

        public ArtistArtworkDto? AllocateArtist(int artistId, int artworkId)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("INSERT INTO artist_artwork(artist_id, artwork_id, modified_at, created_at) VALUES (@artistId, @artworkId, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", artistId);
                    cmd.Parameters.AddWithValue("@artworkId", artworkId);
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? new ArtistArtworkDto(artistId, artworkId) : null;
                }
            }
        }

        public bool DeallocateArtist(int artistId, int artworkId)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                using NpgsqlCommand cmd = new("DELETE FROM artist_artwork WHERE artist_id = @artistId AND artwork_id = @artworkId", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", artistId);
                    cmd.Parameters.AddWithValue("@artworkId", artworkId);
                    int result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}