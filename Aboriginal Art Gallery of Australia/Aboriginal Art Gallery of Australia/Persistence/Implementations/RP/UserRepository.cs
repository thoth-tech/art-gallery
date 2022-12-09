using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;
using BCrypt.Net;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class UserRepository : IRepository, IUserDataAccess
    {

        //TODO: Add data to database and test each of the following methods

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        private IRepository _repo => this;

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

            var result = _repo.ExecuteReader<UserInputDto>("INSERT INTO account(first_name, last_name, email, " +
                "password_hash, role, active_at, modified_at, created_at) VALUES (@firstName, @lastName, @email, " +
                "@passwordHash, @role, @activeAt current_timestamp, current_timestamp);", sqlParams)
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
                new("password_hash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384))
            };

            var result = _repo.ExecuteReader<UserInputDto>("UPDATE account SET first_name = @firstName, last_name = @lastName, " +
                "email = @email, password_hash = @password_hash, modified_at = current_timestamp WHERE account_id = " +
                "@accountId", sqlParams)
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
    }
}
