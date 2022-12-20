using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;
using Aboriginal_Art_Gallery_of_Australia.Authentication;
using BCrypt.Net;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class UserRepository : IRepository, IUserDataAccess
    {

        //TODO: Add data to database and test each of the following methods

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
                new("role", "Member"),
                new("activeAt", (object)DBNull.Value)
            };

            var result = _repo.ExecuteReader<UserInputDto>("INSERT INTO account VALUES (DEFAULT, @firstName, " +
                "@lastName, @email, @passwordHash, @role, @activeAt current_timestamp, current_timestamp) " +
                "RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public UserInputDto? UpdateUser(int id, UserInputDto user)
        {
            // Hacky way to only update one or a few fields rather than all
            var sqlParams1 = new NpgsqlParameter[]
            {
                new("accountId", id),
            };
            var existing = _repo.ExecuteReader<UserOutputDto>("SELECT * FROM account WHERE account_id = @accountId", sqlParams1).SingleOrDefault();
            if (existing is null) { return null; }
            if (user.FirstName == ""){ user.FirstName = existing.FirstName;}
            if (user.LastName == ""){ user.LastName = existing.LastName;}
            if (user.Email == ""){ user.Email = existing.Email;}
            if (user.Password == ""){ user.Password = existing.PasswordHash; }
            else { user.Password = BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384); }

            var sqlParams = new NpgsqlParameter[]
            {
                new("accountId", id),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                new("email", user.Email),
                new("password_hash", user.Password)
            };

            var result = _repo.ExecuteReader<UserInputDto>("UPDATE account SET first_name = @firstName, " +
                "last_name = @lastName, email = @email, password_hash = @password_hash, " +
                "modified_at = current_timestamp WHERE account_id = @accountId RETURNING *", sqlParams)
                .SingleOrDefault();

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

        public Tuple<UserOutputDto, string>? AuthenticateUser(LoginDto login)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("email", login.Email)
            };

            UserOutputDto? user = _repo.ExecuteReader<UserOutputDto>("SELECT * FROM account WHERE email = @email", sqlParams).SingleOrDefault();

            if (user is not null) {
                // Authenticate user with given login information and return an auth token if valid
                bool authenticated = BC.EnhancedVerify(login.Password, user.PasswordHash, hashType: HashType.SHA384);
                if (authenticated)
                {
                    user.PasswordHash = ""; // Removing password hash
                    var handler = new TokenAuthenticationHandler(_configuration);
                    string token = handler.GenerateToken(user);
                    return new Tuple<UserOutputDto, string>(user, token);
                }
            }
            return null;
        }
    }
}
