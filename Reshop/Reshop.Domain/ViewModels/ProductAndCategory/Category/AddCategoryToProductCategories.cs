using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Models.ProductAndCategory;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Category
{
    public class AddCategoryToProductCategories
    {
        public int ProductId { get; set; }
        public IEnumerable<Models.ProductAndCategory.Category> Categories { get; set; }
        public IEnumerable<ChildCategory> ChildCategories { get; set; }

        public IList<int> SelectedCategories { get; set; }
        public IList<int> SelectedChildCategories { get; set; }
    }
}
