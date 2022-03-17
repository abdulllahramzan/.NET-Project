using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Model
{
    public class Product
    {
        [Key]
        public int ProdId { get; set; }
        public string ProdName { get; set; } = "Empty";
        public string ProdPrice { get; set; } = "Empty";
    
    }
}
