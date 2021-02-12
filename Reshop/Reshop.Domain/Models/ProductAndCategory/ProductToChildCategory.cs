using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Models.ProductAndCategory
{
    public class ProductToChildCategory
    {
        public int ProductId { get; set; }
        public int ChildCategoryId { get; set; }

        public Product Product { get; set; }
        public ChildCategory ChildCategory { get; set; }
    }
}
