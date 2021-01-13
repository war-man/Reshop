using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Category
{
    public class AddOrEditCategoryViewModel
    {
        public string UserId { get; set; }

        public Models.ProductAndCategory.Category Category { get; set; }
        public IEnumerable<Models.ProductAndCategory.Product> Products { get; set; }
        public IList<int> SelectedProducts { get; set; }
    }
}
