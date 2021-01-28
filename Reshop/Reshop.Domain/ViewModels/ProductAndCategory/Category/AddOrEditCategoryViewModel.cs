using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Category
{
    public class AddOrEditCategoryViewModel
    {
        public string UserId { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "لطفا نام گروه را وارد کنید")]
        [Display(Name = "نام گروه")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "لطفا توضیحات گروه را وارد کنید")]
        [Display(Name = "توضیحات گروه")]
        public string CategoryDescription { get; set; }

        public IEnumerable<Models.ProductAndCategory.Product> Products { get; set; }
        public IList<int> SelectedProducts { get; set; }
    }
}
