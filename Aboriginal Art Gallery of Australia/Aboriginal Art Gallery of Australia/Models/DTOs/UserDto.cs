namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class UserInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserInputDto(string firstName, string lastName, string email, string password)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
        }

        public UserInputDto(){
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Password = "";
        }
    }

    public class UserOutputDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime? ActiveAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserOutputDto(int id, string firstName, string lastName, string email, string passwordHash, string role, DateTime? activeAt, DateTime modifiedAt, DateTime createdAt)
        {
            this.Id = id;
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