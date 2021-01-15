using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reshop.Domain.Models.User.Cart;
using Reshop.Domain.Models.User.Comment;

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
        public Item Item { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public ICollection<CommentForProduct> CommentsForProduct { get; set; }
        public ICollection<QuestionForProduct> QuestionsForProduct { get; set; }
    }
}
