using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ItemsInStock { get; set; }
        public int ItemsReserved { get; set; }
    }
}
