namespace Art_Gallery_Backend.Models.DTOs
{
    /// <summary>
    /// The LoginDto class is used to decouple the service layer from the database layer. It provides a means of mapping the necessary user credentials to the appropriate database model.
    /// </summary>
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public LoginDto() { }
    }
}
