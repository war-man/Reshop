using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Category
{
    public class AddCategoryToProductCategories
    {
        public int ProductId { get; set; }
        public IEnumerable<Models.ProductAndCategory.Category> Categories { get; set; }
        public IList<int> SelectedCategories { get; set; }
    }
}
