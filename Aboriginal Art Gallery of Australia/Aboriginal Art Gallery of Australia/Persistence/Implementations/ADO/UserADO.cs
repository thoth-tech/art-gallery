using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Aboriginal_Art_Gallery_of_Australia.Authentication;
using BCrypt.Net;

using Npgsql;


namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO
{
    public class UserADO : IUserDataAccess
    {
        private readonly IConfiguration _configuration;

        public UserADO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<UserOutputDto> GetUsers()
        {
            var users = new List<UserOutputDto>();
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM account", connection);
                {
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var userID = (int)dr["account_id"];
                                var firstName = (string)dr["first_name"];
                                var lastName = (string)dr["last_name"];
                                var email = (string)dr["email"];
                                var passwordHash = (string)dr["password_hash"];
                                var role = (string)dr["role"];
                                var activeAt = (DateTime)dr["active_at"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];
                                users.Add(new UserOutputDto(userID, firstName, lastName, email, passwordHash, role, activeAt, modifiedAt, createdAt));
                            }
                        }
                        return users;
                    }
                }
            }
        }

        public UserOutputDto? GetUserById(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM account WHERE account_id = @userID", connection);
                {
                    cmd.Parameters.AddWithValue("@userId", id);
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var userID = (int)dr["account_id"];
                                var firstName = (string)dr["first_name"];
                                var lastName = (string)dr["last_name"];
                                var email = (string)dr["email"];
                                var passwordHash = (string)dr["password_hash"];
                                var role = (string)dr["role"];
                                var activeAt = (DateTime)dr["active_at"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];
                                return new UserOutputDto(userID, firstName, lastName, email, passwordHash, role, activeAt, modifiedAt, createdAt);
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public string? AuthenticateUser(LoginDto login)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM account WHERE email = @email", connection);
                {
                    cmd.Parameters.AddWithValue("@email", login.Email);

                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var userID = (int)dr["account_id"];
                                var firstName = (string)dr["first_name"];
                                var lastName = (string)dr["last_name"];
                                var email = (string)dr["email"];
                                var passwordHash = (string)dr["password_hash"];
                                var role = (string)dr["role"];
                                var activeAt = (DateTime)dr["active_at"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];

                                UserOutputDto user = new UserOutputDto(userID, firstName, lastName, email, passwordHash, role, activeAt, modifiedAt, createdAt);

                                // Authenticate user with given login information and return an auth token if valid
                                bool authenticated = BC.EnhancedVerify(login.Password, user.PasswordHash, hashType: HashType.SHA384);
                                if (authenticated)
                                {
                                    user.PasswordHash = ""; // Removing password hash
                                    var handler = new TokenAuthenticationHandler(_configuration);

                                    return handler.GenerateToken(user);
                                }
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public UserInputDto? InsertUser(UserInputDto user)
        {

            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO account(first_name, last_name, email, password_hash, role, active_at, modified_at, created_at) " +
                                                  "VALUES (@firstName, @lastName, @email, @passwordHash, @role, @activeAt, current_timestamp, current_timestamp);", connection);
                {
                    cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", user.LastName);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@passwordHash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384));
                    cmd.Parameters.AddWithValue("@role", "User");
                    cmd.Parameters.AddWithValue("@activeAt", DateTime.UtcNow);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? user : null;
                }
            }
        }

        public UserInputDto? UpdateUser(int id, UserInputDto user)
        {

            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                String cmdString = "UPDATE account SET ";

                if (user.FirstName is not null && user.FirstName != "" && user.FirstName != "string")
                {
                    cmdString += "first_name = @firstName, ";
                }
                if (user.LastName is not null && user.LastName != "" && user.LastName != "string")
                {
                    cmdString += "last_name = @lastName, ";
                }
                if (user.Email is not null && user.Email != "" && user.Email != "string")
                {
                    cmdString += "email = @email, ";
                }
                if (user.Password is not null && user.Password != "" && user.Password != "string")
                {
                    cmdString += "password_hash = @passwordHash, ";
                }
                if (user.Role is not null && user.Role != "" && user.Password != "string")
                {
                    cmdString += "role = @role, ";
                }

                cmdString += "modified_at = current_timestamp WHERE account_id = @accountId";

                using var cmd = new NpgsqlCommand(cmdString, connection);
                {
                    cmd.Parameters.AddWithValue("@accountId", id);
                    if (user.FirstName is not null && user.FirstName != "" && user.FirstName != "string")
                    {
                        cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                    }
                    if (user.LastName is not null && user.LastName != "" && user.LastName != "string")
                    {
                        cmd.Parameters.AddWithValue("@lastName", user.LastName);
                    }
                    if (user.Email is not null && user.Email != "" && user.Email != "string")
                    {
                        cmd.Parameters.AddWithValue("@email", user.Email);
                    }
                    if (user.Password is not null && user.Password != "" && user.Password != "string")
                    {
                        cmd.Parameters.AddWithValue("@passwordHash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384));
                    }
                    if (user.Role is not null && user.Role != "" && user.Password != "string")
                    {
                        cmd.Parameters.AddWithValue("@role", user.Role);
                    }
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? user : null;
                }
            }
        }

        public bool DeleteUser(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM account WHERE account_id = @accountId", connection);
                {
                    cmd.Parameters.AddWithValue("@accountId", id);
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}