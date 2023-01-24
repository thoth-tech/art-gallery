using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Npgsql;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;


namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO
{
    public class ArtistADO : IArtistDataAccess
    {
        private readonly IConfiguration _configuration;

        public ArtistADO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<ArtistOutputDto> GetArtists()
        {
            List<ArtistOutputDto> artists = new();
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
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
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
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
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
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

        // Option 1: The update function requires all DTO fields to be complete. Meaning database records can not be partially updated.
        public ArtistInputDto? UpdateArtist(int id, ArtistInputDto artist)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
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

        // Option 2: This update function does not require all DTO fields to be complete. Any field left null, “”, or its default value; i.e., string, 0, ect, will not be updated. Nullable fields can not be set back to null after being updated.
        /*public ArtistInputDto? UpdateArtist(int id, ArtistInputDto artist)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                string cmdString = "UPDATE artist SET ";

                if (artist.FirstName is not null and not "" and not "string")
                {
                    cmdString += "first_name = @firstName, ";
                }
                if (artist.LastName is not null and not "" and not "string")
                {
                    cmdString += "last_name = @lastName, ";
                }
                if (artist.DisplayName is not null and not "" and not "string")
                {
                    cmdString += "display_name = @displayName, ";
                }
                if (artist.ProfileImageUrl is not null and not "" and not "string")
                {
                    cmdString += "profile_image_url = @profileImageURL, ";
                }
                if (artist.PlaceOfBirth is not null and not "" and not "string")
                {
                    cmdString += "place_of_birth = @placeOfBirth, ";
                }
                if (artist.YearOfBirth is not null and not 0)
                {
                    cmdString += "year_of_birth = @yearOfBirth, ";
                }
                if (artist.YearOfDeath is not null and not 0)
                {
                    cmdString += "year_of_death = @yearOfDeath, ";
                }

                cmdString += "modified_at = current_timestamp WHERE artist_id = @artistId";


                using NpgsqlCommand cmd = new(cmdString, connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", id);
                    if (artist.FirstName is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@firstName", artist.FirstName);
                    }
                    if (artist.LastName is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@lastName", artist.LastName);
                    }
                    if (artist.DisplayName is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@displayName", artist.DisplayName);
                    }
                    if (artist.ProfileImageUrl is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@profileImageURL", artist.ProfileImageUrl);
                    }
                    if (artist.PlaceOfBirth is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@placeOfBirth", artist.PlaceOfBirth);
                    }
                    if (artist.YearOfBirth is not null and not 0)
                    {
                        cmd.Parameters.AddWithNullableValue("@yearOfBirth", artist.YearOfBirth);
                    }
                    if (artist.YearOfDeath is not null and not 0)
                    {
                        cmd.Parameters.AddWithNullableValue("@yearOfDeath", artist.YearOfDeath);
                    }
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? artist : null;
                }
            }
        }*/

        public bool DeleteArtist(int id)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
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