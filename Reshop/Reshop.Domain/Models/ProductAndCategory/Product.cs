using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Reshop.Domain.Models.User.Cart;

namespace Reshop.Domain.Models.ProductAndCategory
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(4)]
        public string ShortKey { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int ImageId { get; set; }

        public string DateTime { get; set; }


        public ICollection<ProductToCategory> ProductToCategories { get; set; }
        public ICollection<ProductToChildCategory> ProductToChildCategories { get; set; }
        public Item Item { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public IList<CommentForProduct> CommentsForProduct { get; set; }
    }
}
