using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;

namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    /// <summary>
    /// The User class is responsible for handling the database model associated with gallery users. 
    /// </summary>
    public class User : IUserDataAccess
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Role { get; set; } = "Member";
        public DateTime? ActiveAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public User(int accountId, string firstName, string lastName, string email, string passwordHash, string? role, DateTime? activeAt, DateTime modifiedAt, DateTime createdAt)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            ActiveAt = activeAt;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 33 is required for the active record implementation.
        private static string _connectionString = "Host=localhost;Database=Deakin University | AAGoA;Username=postgres;Password=postgreSQL;";

        public User() { }

        public List<UserOutputDto> GetUsers()
        {
            throw new NotImplementedException();
        }

        public UserOutputDto? GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public UserInputDto? InsertUser(UserInputDto user)
        {
            throw new NotImplementedException();
        }

        public UserInputDto? UpdateUser(int id, UserInputDto user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public string? AuthenticateUser(LoginDto login)
        {
            throw new NotImplementedException();
        }
    }
}