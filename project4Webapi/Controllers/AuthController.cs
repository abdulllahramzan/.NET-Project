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
        private readonly IAuthRepository _authRepo;
        private readonly DataContext _context;

        public AuthController(IAuthRepository authRepo, DataContext context)
        {
           _authRepo = authRepo;
            _context = context;
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
           
            //var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions()
            //{
            //    Path = "/",
            //    HttpOnly = true,
            //    IsEssential = true,
            //    Expires = DateTime.Now.AddHours(1),
            //};
            //var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username );
            //Response.Cookies.Append("token", _authRepo.CreateToken(user), cookieOptions);

            return Ok(response);
        }

    }
}
