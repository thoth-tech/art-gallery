namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginDto(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        public LoginDto(){
            this.Email = "";
            this.Password = "";
        }
    }
}