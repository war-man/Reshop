namespace Reshop.Domain.Models.ProductAndCategory
{
    public class ChildCategoryToCategory
    {
        public int CategoryId { get; set; }
        public int ChildCategoryId { get; set; }

        public Category Category { get; set; }
        public ChildCategory ChildCategory { get; set; }
    }
}