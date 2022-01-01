using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TicketStore.Entities;

namespace TicketStore.Managers
{
    public class TokenManager : ITokenManager
    {
        private readonly UserManager<User> _userManager;

        public TokenManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> CreateToken(User user)
        {
            var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            if (jwtSecretKey == null)
            {
                return null;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(100),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}