using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.ViewModels.ProductAndCategory.Category;
using Reshop.Domain.ViewModels.ProductAndCategory.Product;

namespace Reshop.Domain.Services.Interfaces.ProductAndCategory
{
    public interface IProductRe
    {
        Task<ShowProductsViewModel> GetProductsWithPagingAsync(int pageId = 1, int take = 4);
        Task<IEnumerable<Product>> GetProducts();
        Task<DetailViewModel> GetDetailOfProductAsync(int productId, string userId);
        Task<ShowProductsViewModel> ShowProductsByCategoryIdAsync(int categoryId, int take = 20, int pageId = 1);
        Task<ShowProductsViewModel> ShowProductsByChildCategoryId(int childCategoryId, int take = 20, int pageId = 1);
        Task<ShowProductsViewModel> SearchProductByFilterAsync(string productName);
        Task<AddOrEditProductViewModel> GetProductColumnsForEditAsync(int productId, string userId);
        Task EditProductAsync(AddOrEditProductViewModel model);
        Task AddProductAsync(AddOrEditProductViewModel model);
        Task DeleteProductAsync(int productId, string userId);
        Task<Product> FindByIdAsync(int productId);
        Task<Product> FindProductByShortKeyAsync(string key);
        Task<AddOrEditCategoryViewModel> GetAllProductsForAddingCategory();
        Task AddCommentToProduct(CommentForProduct model);
        Task<IEnumerable<CommentForProduct>> GetCommentsOfProduct(int productId, int take = 20);
    }
}
