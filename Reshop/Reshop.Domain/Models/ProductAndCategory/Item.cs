using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Models.ProductAndCategory
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int QuantityInStock { get; set; }


        public Product Product { get; set; }
    }
}
