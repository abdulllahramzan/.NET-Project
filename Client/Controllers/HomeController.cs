
using Client.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
    
    public class HomeController : Controller
    {
        [Authorize]

        public IActionResult Index()
        {

            return View();
        }

       [HttpGet]
        public IActionResult Index(LoginDto loginDto)
        {
            var accesstoken = HttpContext.Session.GetString("JWToken");
            HttpClient client = new HttpClient();
            if (accesstoken == null)
            {
                return Unauthorized();
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accesstoken);


            return View();
        }

       
    }

}

