using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4Webapi.Data;
using project4Webapi.Dtos.User;
using project4Webapi.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace project4Webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAuthRepository _authRepo;

        public AuthController(DataContext context, IAuthRepository authRepo)
        {
            _context = context;
            _authRepo = authRepo;

        }

        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(RegisterDto request)
        {
            var response = await _authRepo.Register(
                new User { Username = request.Username , Password = request.Password }
            );
           
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            var response = await _authRepo.Login(
              request.Username, request.Password
           );

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(request.Username.ToLower()));
            var refreshToken = _authRepo.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            _context.SaveChanges();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
          
            return Ok(response);
        }

        [HttpPost]
        [Route("refresh")]
        public ActionResult Refresh(TokenApiModel tokenApiModel)
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

        //[HttpPost, Authorize]
        //[Route("revoke")]
        //public IActionResult Revoke()
        //{
        //    var username = User.Identity.Name;
        //    var user = _context.Users.SingleOrDefault(u => u.Username == username);
        //    if (user == null) return BadRequest();
        //    user.RefreshToken = null;
        //    _context.SaveChanges();
        //    return NoContent();
        //}
    }
}
