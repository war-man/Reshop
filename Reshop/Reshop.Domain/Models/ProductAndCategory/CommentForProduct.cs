using System;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Models.ProductAndCategory
{
    public class CommentForProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string DateTime { get; set; }

        public int Like { get; set; }

        public Product Product { get; set; }
    }
}