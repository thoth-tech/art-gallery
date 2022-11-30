namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Role { get; set; } = "Guest";
        public DateTime ActiveAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public User(int id, string firstName, string lastName, string email, string passwordHash, string? role, DateTime activeAt, DateTime modifiedAt, DateTime createdAt)
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