using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Reshop.Domain.Models.ProductAndCategory;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Product
{
    public class AddOrEditProductViewModel
    {
        [Required]
        public string UserId { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا نام محصول را وارد کنید")]
        [Display(Name = "نام محصول")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا جزئیات محصول را وارد کنید")]
        [Display(Name = "جزئیات محصول")]
        public string Description { get; set; }

        public string ShortKey { get; set; }

        [Required(ErrorMessage = "لطفا قیمت محصول را وارد کنید")]
        [Display(Name = "قیمت محصول")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "لطفا مقدار موجودی محصول را وارد کنید")]
        [Display(Name = "مقدار موجودی محصول")]
        public int QuantityInStock { get; set; }

        [Display(Name = "تصویر محصول")]
        public IFormFile Picture { get; set; }

        public IEnumerable<Models.ProductAndCategory.Category> Categories { get; set; }
        public List<int> SelectedCategories { get; set; }
    }
}
