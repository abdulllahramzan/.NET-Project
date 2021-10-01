using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project4Webapi.Dtos.User
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
       
    }
}
