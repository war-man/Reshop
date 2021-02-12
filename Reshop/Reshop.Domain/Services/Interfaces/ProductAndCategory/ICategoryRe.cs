using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.ViewModels.ProductAndCategory.Category;
using Reshop.Domain.ViewModels.ProductAndCategory.Product;

namespace Reshop.Domain.Services.Interfaces.ProductAndCategory
{
    public interface ICategoryRe
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<AddOrEditProductViewModel> GetAllCategoriesForAddingProduct(string userId);
        Task<IEnumerable<ShowCategoryViewModel>> GetCategoriesForShowAsync();
        Task<AddCategoryToProductCategories> GetCategoriesThatProductDonotHaveAsync(int id);
        Task AddCategoryToProductCategoriesAsync(AddCategoryToProductCategories model);
        Task AddCategoryAsync(AddOrEditCategoryViewModel model);
        Task<AddOrEditCategoryViewModel> GetCategoryColumnsWithItsProductsAsync(int categoryId);
        Task<Category> FindCategoryByIdAsync(int categoryId);
        Task EditCategoryAsync(AddOrEditCategoryViewModel model);
        Task DeleteCategoryAsync(int categoryId);

        // --------------- child category ---------------

        Task<IEnumerable<ChildCategory>> GetChildCategoriesAsync();
        Task EditChildCategoryAsync(ChildCategory model);
        Task AddChildCategoryAsync(ChildCategory model);
        Task<ChildCategory> GetChildCategoryColumnsAsync(int childCategoryId);
        Task<AddChildCategoryToCategoryViewModel> GetChildCategoriesThatCategoryDonotHaveAsync(int categoryId);
        Task AddChildCategoryToCategory(AddChildCategoryToCategoryViewModel model);
    }
}