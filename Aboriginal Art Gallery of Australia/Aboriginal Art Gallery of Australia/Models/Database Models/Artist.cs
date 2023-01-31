using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Npgsql;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;

namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    /// <summary>
    /// The Artist class is responsible for handling the database model associated with artists. The Second half of the Artist class from line 37 down is the implementation of the Active Record design pattern. This is a duplication of code from the ADO implementation for OOP demonstration purposes.
    /// </summary>
    public class Artist : IArtistDataAccess
    {
        public int ArtistId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ProfileImageURL { get; set; } = string.Empty;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public int YearOfBirth { get; set; }
        public int? YearOfDeath { get; set; } = null;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Artist(int artistId, string firstName, string lastName, string displayName, string profileImageURL, string placeOfBirth, int yearOfBirth, int? yearOfDeath, DateTime modifiedAt, DateTime createdAt)
        {
            ArtistId = artistId;
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            ProfileImageURL = profileImageURL;
            PlaceOfBirth = placeOfBirth;
            YearOfBirth = yearOfBirth;
            YearOfDeath = yearOfDeath;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 37 is required for the active record implementation.
        private static readonly string _connectionString = "Host=localhost;Database=Deakin University | AAGoA;Username=postgres;Password=postgreSQL;";

        public Artist() { }

        public List<ArtistOutputDto> GetArtists()
        {
            List<ArtistOutputDto> artists = new();
            using NpgsqlConnection connection = new(_connectionString);
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM artist", connection);
                {
                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int artistId = (int)dr["artist_id"];
                                string firstName = (string)dr["first_name"];
                                string lastName = (string)dr["last_name"];
                                string displayName = (string)dr["display_name"];
                                string profileImageURL = (string)dr["profile_image_url"];
                                string placeOfBirth = (string)dr["place_of_birth"];
                                int yearOfBirth = (int)dr["year_of_birth"];
                                int? yearOfDeath = ConvertFromNullableValue<int?>(dr["year_of_death"]);
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];
                                artists.Add(new ArtistOutputDto(artistId, firstName, lastName, displayName, profileImageURL, placeOfBirth, yearOfBirth, yearOfDeath, modifiedAt, createdAt));
                            }
                        }
                        return artists;
                    }
                }
            }
        }

        public ArtistOutputDto? GetArtistById(int id)
        {
            using NpgsqlConnection connection = new(_connectionString);
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM artist WHERE artist_id = @artistId", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", id);
                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int artistId = (int)dr["artist_id"];
                                string firstName = (string)dr["first_name"];
                                string lastName = (string)dr["last_name"];
                                string displayName = (string)dr["display_name"];
                                string profileImageURL = (string)dr["profile_image_url"];
                                string placeOfBirth = (string)dr["place_of_birth"];
                                int yearOfBirth = (int)dr["year_of_birth"];
                                int? yearOfDeath = ConvertFromNullableValue<int?>(dr["year_of_death"]);
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];
                                return new ArtistOutputDto(artistId, firstName, lastName, displayName, profileImageURL, placeOfBirth, yearOfBirth, yearOfDeath, modifiedAt, createdAt);
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public ArtistInputDto? InsertArtist(ArtistInputDto artist)
        {
            using NpgsqlConnection connection = new(_connectionString);
            {
                connection.Open();
                using NpgsqlCommand cmd = new("INSERT INTO artist(first_name, last_name, display_name, profile_image_url,place_of_birth, year_of_birth, year_of_death, modified_at, created_at) " +
                                              "VALUES (@firstName, @lastName, @displayName, @profileImageURL, @placeOfBirth, @yearOfBirth, @yearOfDeath, current_timestamp, current_timestamp);", connection);
                {
                    cmd.Parameters.AddWithValue("@firstName", artist.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", artist.LastName);
                    cmd.Parameters.AddWithValue("@displayName", artist.DisplayName);
                    cmd.Parameters.AddWithValue("@profileImageURL", artist.ProfileImageUrl);
                    cmd.Parameters.AddWithValue("@placeOfBirth", artist.PlaceOfBirth);
                    cmd.Parameters.AddWithNullableValue("@yearOfBirth", artist.YearOfBirth);
                    cmd.Parameters.AddWithNullableValue("@yearOfDeath", artist.YearOfDeath);
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? artist : null;
                }
            }
        }

        public ArtistInputDto? UpdateArtist(int id, ArtistInputDto artist)
        {
            using NpgsqlConnection connection = new(_connectionString);
            {
                connection.Open();
                using NpgsqlCommand cmd = new("UPDATE artist " +
                                              "SET first_name = @firstName, " +
                                                "last_name = @lastName, " +
                                                "display_name = @displayName, " +
                                                "profile_image_url = @profileImageURL, " +
                                                "place_of_birth = @placeOfBirth, " +
                                                "year_of_birth = @yearOfBirth, " +
                                                "year_of_death = @yearOfDeath, " +
                                                "modified_at = current_timestamp " +
                                              "WHERE artist_id = @artistId", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", id);
                    cmd.Parameters.AddWithValue("@firstName", artist.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", artist.LastName);
                    cmd.Parameters.AddWithValue("@displayName", artist.DisplayName);
                    cmd.Parameters.AddWithValue("@profileImageURL", artist.ProfileImageUrl);
                    cmd.Parameters.AddWithValue("@placeOfBirth", artist.PlaceOfBirth);
                    cmd.Parameters.AddWithNullableValue("@yearOfBirth", artist.YearOfBirth);
                    cmd.Parameters.AddWithNullableValue("@yearOfDeath", artist.YearOfDeath);
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? artist : null;
                }
            }
        }

        public bool DeleteArtist(int id)
        {
            using NpgsqlConnection connection = new(_connectionString);
            {
                connection.Open();
                using NpgsqlCommand cmd = new("DELETE FROM artist WHERE artist_id = @artistId", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", id);
                    int result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}