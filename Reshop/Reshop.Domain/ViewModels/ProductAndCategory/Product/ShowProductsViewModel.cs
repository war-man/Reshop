using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Product
{
    public class ShowProductsViewModel
    {
        public ICollection<Models.ProductAndCategory.Product> Products { get; set; }

        // pagination
        public int PageId { get; set; }
        public double PageCount { get; set; }
    }
}
