using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4Webapi.Data;
using project4Webapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project4Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly DataContext _context;
        private readonly IAuthRepository _authRepo;

        public TokenController(DataContext context, IAuthRepository authRepo)
        {
            _context = context;
            _authRepo = authRepo;
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _authRepo.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }
            var newAccessToken = _authRepo.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _authRepo.GenerateRefreshToken();
 
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            Response.Cookies.Append("RefreshToken", newRefreshToken, cookieOptions);
           
            user.RefreshToken = newRefreshToken;
            _context.SaveChanges();
            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken,
                
            });
        }
     

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
