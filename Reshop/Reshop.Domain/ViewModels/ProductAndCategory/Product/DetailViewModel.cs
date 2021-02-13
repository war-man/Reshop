using System.Collections.Generic;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Models.User.Comment;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Product
{
    public class DetailViewModel
    {
        public string UserId { get; set; }
        public Models.ProductAndCategory.Product Product { get; set; }
        public List<Models.ProductAndCategory.Category> Categories { get; set; }
        public IEnumerable<CommentForProduct> CommentsForProduct { get; set; }
    }
}
