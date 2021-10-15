using project4Webapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace project4Webapi.Data
{
    public interface IAuthRepository
    {
        Task<string> Register(User user);
        Task<string> Login(string username, string password);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        //Task<bool> UserExists(string username);

    }

}
