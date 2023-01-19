using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aboriginal_Art_Gallery_of_Australia.Authentication
{
    /// <summary>
    /// The TokenAuthenticationHandler class is responsible for generating user claims and JSON Web Tokens (JWT) upon user authentication to provide user authorization.
    /// </summary>
    public class TokenAuthenticationHandler
    {
        private readonly IConfiguration _configuration;

        public TokenAuthenticationHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Initializers a new instance of the claimsIdentity as well as generates a JWT for user authorization for a set period of time.
        /// </summary>
        /// <param name="user">The User Authorization is to be generated for.</param>
        /// <returns> The JSON Web Token as a string.</returns>
        public string GenerateToken(UserOutputDto user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}