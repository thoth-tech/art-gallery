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
            var artists = new List<ArtistOutputDto>();
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM artist", connection);
                {
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var artistId = (int)dr["artist_id"];
                                var firstName = (string)dr["first_name"];
                                var lastName = (string)dr["last_name"];
                                var displayName = (string)dr["display_name"];
                                var profileImageURL = (string)dr["profile_image_url"];
                                var placeOfBirth = (string)dr["place_of_birth"];
                                var yearOfBirth = (int)dr["year_of_birth"];
                                var yearOfDeath = ConvertFromNullableValue<int?>(dr["year_of_death"]);
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];
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
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM artist WHERE artist_id = @artistId", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", id);
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var artistId = (int)dr["artist_id"];
                                var firstName = (string)dr["first_name"];
                                var lastName = (string)dr["last_name"];
                                var displayName = (string)dr["display_name"];
                                var profileImageURL = (string)dr["profile_image_url"];
                                var placeOfBirth = (string)dr["place_of_birth"];
                                var yearOfBirth = (int)dr["year_of_birth"];
                                var yearOfDeath = ConvertFromNullableValue<int?>(dr["year_of_death"]);
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];
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
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO artist(first_name, last_name, display_name, profile_image_url,place_of_birth, year_of_birth, year_of_death, modified_at, created_at) " +
                                                  "VALUES (@firstName, @lastName, @displayName, @profileImageURL, @placeOfBirth, @yearOfBirth, @yearOfDeath, current_timestamp, current_timestamp);", connection);
                {
                    cmd.Parameters.AddWithValue("@firstName", artist.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", artist.LastName);
                    cmd.Parameters.AddWithValue("@displayName", artist.DisplayName);
                    cmd.Parameters.AddWithValue("@profileImageURL", artist.ProfileImageURL);
                    cmd.Parameters.AddWithValue("@placeOfBirth", artist.PlaceOfBirth);
                    cmd.Parameters.AddWithNullableValue("@yearOfBirth", artist.YearOfBirth);
                    cmd.Parameters.AddWithNullableValue("@yearOfDeath", artist.YearOfDeath);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? artist : null;
                }
            }
        }

        public ArtistInputDto? UpdateArtist(int id, ArtistInputDto artist)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                String cmdString = "UPDATE artist SET ";

                if (artist.FirstName is not null && artist.FirstName != "" && artist.FirstName != "string")
                {
                    cmdString += "first_name = @firstName, ";
                }
                if (artist.LastName is not null && artist.LastName != "" && artist.LastName != "string")
                {
                    cmdString += "last_name = @lastName, ";
                }
                if (artist.DisplayName is not null && artist.DisplayName != "" && artist.DisplayName != "string")
                {
                    cmdString += "display_name = @displayName, ";
                }
                if (artist.ProfileImageURL is not null && artist.ProfileImageURL != "" && artist.ProfileImageURL != "string")
                {
                    cmdString += "profile_image_url = @profileImageURL, ";
                }
                if (artist.PlaceOfBirth is not null && artist.PlaceOfBirth != "" && artist.PlaceOfBirth != "string")
                {
                    cmdString += "place_of_birth = @placeOfBirth, ";
                }
                if (artist.YearOfBirth is not null && artist.YearOfBirth != 0)
                {
                    cmdString += "year_of_birth = @yearOfBirth, ";
                }
                if (artist.YearOfDeath is not null && artist.YearOfDeath != 0)
                {
                    cmdString += "year_of_death = @yearOfDeath, ";
                }

                cmdString += "modified_at = current_timestamp WHERE artist_id = @artistId";


                using var cmd = new NpgsqlCommand(cmdString, connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", id);
                    if (artist.FirstName is not null && artist.FirstName != "" && artist.FirstName != "string")
                    {
                        cmd.Parameters.AddWithValue("@firstName", artist.FirstName);
                    }
                    if (artist.LastName is not null && artist.LastName != "" && artist.LastName != "string")
                    {
                        cmd.Parameters.AddWithValue("@lastName", artist.LastName);
                    }
                    if (artist.DisplayName is not null && artist.DisplayName != "" && artist.DisplayName != "string")
                    {
                        cmd.Parameters.AddWithValue("@displayName", artist.DisplayName);
                    }
                    if (artist.ProfileImageURL is not null && artist.ProfileImageURL != "" && artist.ProfileImageURL != "string")
                    {
                        cmd.Parameters.AddWithValue("@profileImageURL", artist.ProfileImageURL);
                    }
                    if (artist.PlaceOfBirth is not null && artist.PlaceOfBirth != "" && artist.PlaceOfBirth != "string")
                    {
                        cmd.Parameters.AddWithValue("@placeOfBirth", artist.PlaceOfBirth);
                    }
                    if (artist.YearOfBirth is not null && artist.YearOfBirth != 0)
                    {
                        cmd.Parameters.AddWithNullableValue("@yearOfBirth", artist.YearOfBirth);
                    }
                    if (artist.YearOfDeath is not null && artist.YearOfDeath != 0)
                    {
                        cmd.Parameters.AddWithNullableValue("@yearOfDeath", artist.YearOfDeath);
                    }
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? artist : null;
                }
            }
        }

        public bool DeleteArtist(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM artist WHERE artist_id = @artistId", connection);
                {
                    cmd.Parameters.AddWithValue("@artistId", id);
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}