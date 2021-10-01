using Client.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project4Webapi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class ProductController : Controller
    {
        
        private static string baseUrl = "https://localhost:44369/Product/Get";

        public  async Task<ActionResult<Product>> Product()
        {
            var product = await GetProduct();
            return View(product);
        }

        [HttpGet]
        public async Task<List<Product>> GetProduct()
        {
            var accesstoken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            string jsonStr = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<List<Product>>(jsonStr).ToList();
            return res;
        }
    }
}

