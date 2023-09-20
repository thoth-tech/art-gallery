using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence.Interfaces;
using static Art_Gallery_Backend.Persistence.ExtensionMethods;
using Art_Gallery_Backend.Authentication;
using Npgsql;
using BCrypt.Net;
using System.Globalization;
using Art_Gallery_Backend.Models.Database_Models;

namespace Art_Gallery_Backend.Persistence.Implementations.RP
{
    public class UserRepository : IRepository, IUserDataAccessAsync
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        public async Task<List<UserOutputDto>> GetUsersAsync()
        {
            var users = await _repo.ExecuteReaderAsync<UserOutputDto>("SELECT * FROM account");
            return users;
        }

        public async Task<UserOutputDto?> GetUserByIdAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("accountId", id)
            };

            var users = await _repo.ExecuteReaderAsync<UserOutputDto>("SELECT * FROM account WHERE account_id = @accountId", sqlParams);
            var user = users.SingleOrDefault();

            return user;
        }

        public async Task<UserInputDto?> InsertUserAsync(UserInputDto user)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("firstName", textInfo.ToTitleCase(user.FirstName)),
                new("lastName", textInfo.ToTitleCase(user.LastName)),
                new("email", user.Email),
                new("passwordHash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384)),
                new("role", textInfo.ToTitleCase(user.Role)),
                new("activeAt", DateTime.UtcNow)
            };

            var results = await _repo.ExecuteReaderAsync<UserInputDto>("INSERT INTO account VALUES (DEFAULT, @firstName, " +
                "@lastName, @email, @passwordHash, @role, @activeAt, current_timestamp, current_timestamp) " +
                "RETURNING *", sqlParams);

            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<UserInputDto?> UpdateUserAsync(int id, UserInputDto user)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("accountId", id),
                new("firstName", textInfo.ToTitleCase(user.FirstName)),
                new("lastName", textInfo.ToTitleCase(user.LastName)),
                new("email", user.Email),
                new("passwordHash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384)),
                new("role", textInfo.ToTitleCase(user.Role))
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

            var results = await _repo.ExecuteReaderAsync<UserInputDto>(cmdString, sqlParams);
            var result = results.SingleOrDefault();

            return result;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("accountId", id)
            };

            var result = await _repo.ExecuteReaderAsync<UserOutputDto>("DELETE FROM account WHERE account_id = @accountId", sqlParams);

            if (result is not null) return true;
            else return false;
        }

        public async Task<string?> AuthenticateUserAsync(LoginDto login)
        {
           var sqlParams = new NpgsqlParameter[]
            {
                new("email", login.Email)
            };

            var users = await _repo.ExecuteReaderAsync<UserOutputDto>("SELECT * FROM account WHERE email = @email", sqlParams);
            var user = users.SingleOrDefault();

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
