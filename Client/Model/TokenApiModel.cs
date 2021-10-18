using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Model
{
    public class TokenApiModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
