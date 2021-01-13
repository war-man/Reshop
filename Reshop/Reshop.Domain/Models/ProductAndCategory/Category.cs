using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Models.ProductAndCategory
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public ICollection<ProductToCategory> ProductToCategory { get; set; }
    }
}
