namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class UserInputDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; }

        public UserInputDto(string firstName, string lastName, string email, string password, string role)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
            this.Role = role;
        }

        public UserInputDto(){
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Password = "";
            this.Role = "";
        }
    }

    public class UserOutputDto
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime? ActiveAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserOutputDto(int accountId, string firstName, string lastName, string email, string passwordHash, string role, DateTime? activeAt, DateTime modifiedAt, DateTime createdAt)
        {
            this.AccountId = accountId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PasswordHash = passwordHash;
            this.Role = role;
            this.ActiveAt = activeAt;
            this.ModifiedAt = modifiedAt;
            this.CreatedAt = createdAt;
        }

        public UserOutputDto(){
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.PasswordHash = "";
            this.Role = "";
        }
    }
}