using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO
{
    public class TidyUp
    {
        private readonly IConfiguration _configuration;

        public TidyUp(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ArtistInputDto? UpdateArtist(int id, ArtistInputDto artist)
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
        }
    }
}
