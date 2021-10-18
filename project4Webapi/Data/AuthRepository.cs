using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using project4Webapi.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace project4Webapi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        private readonly IConfiguration _configuration;
        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;

        }
        public async Task<string> Login(string username, string password)
        { 
            
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
            
            if (user == null)
            {
               string response = "User not Found.";
                return response;
            }
            else if(user.Username == username && user.Password == password)
            {
                
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

                var accessToken = GenerateAccessToken(claims);
               
                string res = accessToken;
                return res; 

            }
            string response1 = "Wrong  Username or Password";
            return response1;
        }
      
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = creds

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)),
                ValidateLifetime = false 
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            //if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            //    throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public async Task<string> Register(User user)
        {
            bool userAlreadyExists = await _context.Users.AnyAsync(x => x.Username == user.Username);
            if(userAlreadyExists == true)
            {
                string response = "user already exists.";
                return response;
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            string response1 = user.Username + " " + "is registered successfully";
            return response1;
        }


        //public async Task<bool> UserExists(string username)
        //{
        //    if (await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower())))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //private string CreateToken(User user)
        // {
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(ClaimTypes.Name, user.Username)
        //    };
        //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("hey_my_top_secret_key"));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = System.DateTime.Now.AddDays(1),
        //        SigningCredentials = creds
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    return tokenHandler.WriteToken(token);
        //}

    }
}
