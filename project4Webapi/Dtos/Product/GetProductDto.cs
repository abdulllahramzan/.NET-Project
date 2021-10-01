using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project4Webapi.Model;
using System.Threading.Tasks;

namespace project4Webapi.Dtos
{
    public class GetProductDto
    {
        public int ProdId { get; set; }
        public string ProdName { get; set; } = "Empty";
        public string ProdPrice { get; set; } = "Empty";
    }
}
