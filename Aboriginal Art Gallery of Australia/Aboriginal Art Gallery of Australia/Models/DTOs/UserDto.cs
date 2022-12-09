namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class UserInputDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public UserInputDto()
        {
        }

        public UserInputDto(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }
    }

    public class UserOutputDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Role { get; set; } = null;
        public DateTime? ActiveAt { get; set; } = null;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserOutputDto()
        {
        }

        public UserOutputDto(int id, string firstName, string lastName, string email, string passwordHash, string? role, DateTime? activeAt, DateTime modifiedAt, DateTime createdAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            ActiveAt = activeAt;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}