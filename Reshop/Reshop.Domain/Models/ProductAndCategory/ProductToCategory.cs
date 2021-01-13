using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Models.ProductAndCategory
{
    public class ProductToCategory
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }


        public Category Category { get; set; }
        public Product Product { get; set; }
    }
}
