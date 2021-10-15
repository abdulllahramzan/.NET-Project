using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project4Webapi.Model
{
    public class User
    {

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public List<Product> Products { get; set; }

        [Required]
        public string Role { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
