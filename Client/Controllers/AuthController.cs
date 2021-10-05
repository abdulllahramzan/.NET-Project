
using Client.Dtos;
using Client.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public  async Task<ActionResult> Register(string username, RegisterDto registerDto)
        {
      
            if (ModelState.IsValid )
            {
                HttpClient hc = new HttpClient();
                hc.BaseAddress = new Uri("https://localhost:44369/Auth/Register");
                var insertrec = await hc.PostAsJsonAsync<RegisterDto>("/Auth/Register", registerDto);
                if (insertrec.IsSuccessStatusCode)
                {
                    ViewBag.Message = "The User " + " ' " + registerDto.Username + " ' " + "is Registered succesfully";
                    return View();
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            //ViewBag.invalid = "The User " + " ' " + registerDto.Username + " ' " + "is Already Exists !!";
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            using (var client = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
                using(var response = await client.PostAsync("https://localhost:44369/Auth/Login", content))
                {
                   loginDto.Token = await response.Content.ReadAsStringAsync();
                    var token = loginDto.Token;
                    HttpContext.Session.SetString("JWToken", token);
                    
                    if(token == "User not Found.")
                    {
                        ViewBag.Message = "Wrong User or Password";
                        return View();
                    }
                    return Redirect("~/Product/Product");
                }
                 
            }
            
        }
        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Auth/Login");
        }

        

    }
}
