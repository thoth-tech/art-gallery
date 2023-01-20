using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Aboriginal_Art_Gallery_of_Australia.Authentication;
using Npgsql;
using BCrypt.Net;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class UserRepository : IRepository, IUserDataAccess
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public List<UserOutputDto> GetUsers()
        {
            var users = _repo.ExecuteReader<UserOutputDto>("SELECT * FROM account");
            return users;
        }

        public UserOutputDto? GetUserById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("accountId", id)
            };

            var user = _repo.ExecuteReader<UserOutputDto>("SELECT * FROM account WHERE account_id = @accountId", sqlParams)
                .SingleOrDefault();

            return user;
        }

        public UserInputDto? InsertUser(UserInputDto user)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                new("email", user.Email),
                new("passwordHash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384)),
                new("role", "User"),
                new("activeAt", DateTime.UtcNow)
            };

            var result = _repo.ExecuteReader<UserInputDto>("INSERT INTO account VALUES (DEFAULT, @firstName, " +
                "@lastName, @email, @passwordHash, @role, @activeAt, current_timestamp, current_timestamp) " +
                "RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public UserInputDto? UpdateUser(int id, UserInputDto user)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("accountId", id),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                new("email", user.Email),
                new("password_hash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384)),
                new("role", user.Role)
            };

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

            cmdString += "modified_at = current_timestamp WHERE account_id = @accountId RETURNING *";

            var result = _repo.ExecuteReader<UserInputDto>(cmdString, sqlParams).SingleOrDefault();

            return result;
        }

        public bool DeleteUser(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("accountId", id)
            };

            _repo.ExecuteReader<UserOutputDto>("DELETE FROM account WHERE account_id = @accountId", sqlParams);

            return true;
        }

        public string? AuthenticateUser(LoginDto login)
        {
           var sqlParams = new NpgsqlParameter[]
            {
                new("email", login.Email)
            };

            var user = _repo.ExecuteReader<UserOutputDto>("SELECT * FROM account WHERE email = @email", sqlParams).SingleOrDefault();

            // Authenticate user with given login information and return an auth token if valid
            if (user != null)
            {
                bool authenticated = BC.EnhancedVerify(login.Password, user.PasswordHash, hashType: HashType.SHA384);
                if (authenticated)
                {
                    user.PasswordHash = ""; // Removing password hash
                    var handler = new TokenAuthentication(_configuration);

                    return handler.GenerateToken(user);
                }
            }

            return null;
        }
    }
}
