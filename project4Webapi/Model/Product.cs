using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project4Webapi.Model
{
    public class Product
    {
        [Key]
        public int ProdId { get; set; }
        public string ProdName { get; set; } = "Empty";
        public string ProdPrice { get; set; } = "Empty";
        public User User { get; set; }
    }
}
