using System.Collections.Generic;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Product
{
    public class DetailViewModel
    {
        public string UserId { get; set; }
        public Models.ProductAndCategory.Product Product { get; set; }
        public List<Models.ProductAndCategory.Category> Categories { get; set; }
    }
}
