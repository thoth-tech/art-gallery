using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Aboriginal_Art_Gallery_of_Australia.Authentication
{
    /// <summary>
    /// The TokenAuthentication class is responsible for handling the access token authentication by generating
    /// JSON web tokens.
    /// </summary>
    public class TokenAuthentication
    {
        private readonly IConfiguration _configuration;

        public TokenAuthentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a token for the given user.
        /// Then length of time before a token expires is 60 minutes.
        /// Signing is done using the configuration settings in appsettings.json, which should
        /// be kept secret.
        /// </summary>
        /// <param name="user">The data for the user who will be authenticated.</param>
        /// <returns>A string containing the token.</returns>
        public string GenerateToken(UserOutputDto user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("Id", "1"),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role)

                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(60),
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}