using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using project4Webapi.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace project4Webapi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context )
        {
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
                new Claim(ClaimTypes.Name, user.Username)
            };
                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("hey_my_top_secret_key"));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = System.DateTime.Now.AddDays(1),
                        SigningCredentials = creds
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                
                    string response =tokenHandler.WriteToken(token);
                    return response;
                
            }
            string response1 = "Wrong  Username or Password";
            return response1;
        }

        public async Task<string> Register(User user)
        { 
            if(await UserExists(user.Username))
            {
                string response = "user already exists.";
                return response;
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            string response1 = user.Username + " " + "is registered successfully";
            return response1;
        }


        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }

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
