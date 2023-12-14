using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _config;
        public TokenRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string CreateJWTTokent(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.UserName));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires : DateTime.Now.AddMinutes(15),
                signingCredentials: credentials                
            );

            // used to convert a JwtSecurityToken object into its corresponding string representation, which is the JSON Web Token (JWT) itself. 
            return new JwtSecurityTokenHandler().WriteToken(token);
            // new JwtSecurityTokenHandler() => This creates an instance of the JwtSecurityTokenHandler class. This class is responsible for handling the creation, validation, and manipulation of JWTs.
            // .WriteToken(token): It serializes the token into its string representation, which is the actual JWT  takes a JwtSecurityToken as an argument.

        }
    }
}
