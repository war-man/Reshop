using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Models.User.Comment;
using Reshop.Domain.ViewModels.ProductAndCategory.Category;
using Reshop.Domain.ViewModels.ProductAndCategory.Product;

namespace Reshop.Domain.Services.Interfaces.ProductAndCategory
{
    public interface IProductRe
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<DetailViewModel> GetDetailOfProductAsync(int productId, string userId);
        Task<IEnumerable<Product>> ShowProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductByFilterAsync(string productName);
        Task<AddOrEditProductViewModel> GetProductColumnsForEditProductAsync(int productId, string userId);
        Task EditProductAsync(AddOrEditProductViewModel model);
        Task AddProductAsync(AddOrEditProductViewModel model);
        Task DeleteProductAsync(int productId, string userId);
        Task<Product> FindByIdAsync(int productId);
        Task<Product> FindProductByShortKeyAsync(string key);
        Task<AddOrEditCategoryViewModel> GetAllProductsForAddingCategory();
        Task AddCommentToProduct(CommentForProduct model);
        Task AddQuestionToProduct(QuestionForProduct model);
        Task AnswerToQuestionInProduct(AnswerToQuestion model);
        Task<Product> FindProductByQuestionId(int questionId);
    }
}
