namespace Art_Gallery_Backend.Models.DTOs
{
    /// <summary>
    /// The UserInputDto class is used to decouple the service layer from the database layer. It provides a means of mapping the necessary user input to the appropriate database model.
    /// </summary>
    public class UserInputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public UserInputDto(string firstName, string lastName, string email, string password, string role)
        {
            if (role == string.Empty) role = "User";

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
        }

        public UserInputDto() { }
    }

    /// <summary>
    /// The UserOutputDto class is used to decouple the database layer from the service layer. It provides a means of mapping the necessary information from the database to the appropriate user output.
    /// </summary>
    public class UserOutputDto
    {
        public int AccountId { get; set; } = 0;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime? ActiveAt { get; set; } = new DateTime();
        public DateTime ModifiedAt { get; set; } = new DateTime();
        public DateTime CreatedAt { get; set; } = new DateTime();

        public UserOutputDto(int accountId, string firstName, string lastName, string email, string passwordHash, string role, DateTime? activeAt, DateTime modifiedAt, DateTime createdAt)
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

        public UserOutputDto() { }
    }
}
