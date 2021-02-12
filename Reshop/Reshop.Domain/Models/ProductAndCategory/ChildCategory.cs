using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Models.ProductAndCategory
{
    public class ChildCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام زیر گروه")]
        [Required(ErrorMessage = "لطفا نام زیر گروه را وارد کنید")]
        public string Name { get; set; }


        public ICollection<ChildCategoryToCategory> ChildCategoryToCategories { get; set; }
        public ICollection<ProductToChildCategory> ProductToChildCategories { get; set; }
    }
}