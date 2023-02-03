using System.Globalization;
using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence;
using Art_Gallery_Backend.Persistence.Interfaces;
using Npgsql;
using static Art_Gallery_Backend.Persistence.ExtensionMethods;

namespace Art_Gallery_Backend.Models.Database_Models
{
    /// <summary>
    /// The Artworks class is responsible for handling the database model associated with artworks. The Second half of the Artworks class from line 36 down is the implementation of the Active Record design pattern. This is a duplication of code from the ADO implementation for OOP demonstration purposes.
    /// </summary>
    public class Artwork : IArtworkDataAccess
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PrimaryImageURL { get; set; } = string.Empty;
        public string? SecondaryImageURL { get; set; } = null;
        public int? YearCreated { get; set; } = null;
        public int? MediaId { get; set; } = null;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Artwork(int artworkId, string title, string description, string primaryImageURL, string? secondaryImageURL, int? yearCreated, int? mediaId, DateTime modifiedAt, DateTime createdAt)
        {
            ArtworkId = artworkId;
            Title = title;
            Description = description;
            PrimaryImageURL = primaryImageURL;
            SecondaryImageURL = secondaryImageURL;
            YearCreated = yearCreated;
            MediaId = mediaId;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 36 is required for the active record implementation.
        private static readonly string _connectionString = "Host=localhost;Database=art_gallery_db;Username=postgres;Password=PostgreSQL;";

        public Artwork() { }

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        public List<ArtworkOutputDto> GetArtworks()
        {
            List<ArtworkOutputDto> artworks = new();
            List<KeyValuePair<int, string>> allArtworkArtists = new();
            using NpgsqlConnection connection = new(_connectionString);
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
                                string? secondaryImageURL = ConvertFromNullableValue<string?>(dr["secondary_image_url"]);
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
            using NpgsqlConnection connection = new(_connectionString);
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
                                string? secondaryImageURL = ConvertFromNullableValue<string?>(dr["secondary_image_url"]);
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
            using NpgsqlConnection connection = new(_connectionString);
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
            using NpgsqlConnection connection = new(_connectionString);
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

        public bool DeleteArtwork(int id)
        {
            using NpgsqlConnection connection = new(_connectionString);
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
            using NpgsqlConnection connection = new(_connectionString);
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
            using NpgsqlConnection connection = new(_connectionString);
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