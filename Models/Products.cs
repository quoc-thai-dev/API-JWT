using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JWT.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public decimal UnitPrice { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
