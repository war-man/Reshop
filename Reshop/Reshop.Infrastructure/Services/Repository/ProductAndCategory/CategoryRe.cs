using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Services.Interfaces.ProductAndCategory;
using Reshop.Domain.ViewModels.ProductAndCategory.Category;
using Reshop.Domain.ViewModels.ProductAndCategory.Product;
using Reshop.Infrastructure.Context;

namespace Reshop.Infrastructure.Services.Repository.ProductAndCategory
{
    public class CategoryRe : ICategoryRe
    {
        private readonly ReshopDbContext _context;

        public CategoryRe(ReshopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        =>
            await _context.Categories.ToListAsync();

        public async Task<AddOrEditProductViewModel> GetAllCategoriesForAddingProduct(string userId)
        {
            var categories = await _context.Categories.ToListAsync();

            return new AddOrEditProductViewModel()
            {
                UserId = userId,
                Categories = categories
            };

        }

        public async Task<IEnumerable<ShowCategoryViewModel>> GetCategoriesForShowAsync()
        =>
            await _context.Categories
                .Select(c => new ShowCategoryViewModel()
                {
                    CategoryId = c.Id,
                    Name = c.Name,
                    ProductCount = _context.ProductToCategories.Count(g => g.CategoryId == c.Id)
                }).ToListAsync();

        public async Task<AddCategoryToProductCategories> GetCategoriesThatProductDonotHaveAsync(int id)
        {
            var productCategories = await _context.Products
                .Where(c => c.Id == id)
                .SelectMany(c => c.ProductToCategories)
                .Select(ca => ca.Category).ToListAsync();

            var categories = await _context.Categories
                .Where(c => !productCategories.Contains(c))
                .ToListAsync();

            return new AddCategoryToProductCategories()
            {
                ProductId = id,
                Categories = categories
            };
        }

        public async Task AddCategoryToProductCategoriesAsync(AddCategoryToProductCategories model)
        {
            foreach (var productToCategory in model.SelectedCategories
                .Select(i => new ProductToCategory()
                {
                    ProductId = model.ProductId,
                    CategoryId = i
                }))
            {
                await _context.AddAsync(productToCategory);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddCategoryAsync(AddOrEditCategoryViewModel model)
        {
            var ca = new Category()
            {
                Name = model.Category.Name,
                Description = model.Category.Description
            };
            await _context.Categories.AddAsync(ca);
            await _context.SaveChangesAsync();

            if (model.SelectedProducts != null)
            {
                foreach (var item in model.SelectedProducts)
                {
                    var productToCategory = new ProductToCategory()
                    {
                        CategoryId = ca.Id,
                        ProductId = item
                    };
                    await _context.AddAsync(productToCategory);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AddOrEditCategoryViewModel> GetCategoryColumnsWithItsProductsAsync(int categoryId)
        {
            var category = await FindCategoryByIdAsync(categoryId);

            var products = await _context.Categories
                .Where(c => c.Id == categoryId)
                .SelectMany(c => c.ProductToCategory)
                .Select(c => new Product()
                {
                    Id = c.Product.Id,
                    Name = c.Product.Name
                }).ToListAsync();

            return new AddOrEditCategoryViewModel()
            {
                Category = category,
                Products = products
            };
        }

        public async Task<Category> FindCategoryByIdAsync(int categoryId)
        =>
            await _context.Categories.FindAsync(categoryId);

        public async Task EditCategoryAsync(AddOrEditCategoryViewModel model)
        {
            var category = await FindCategoryByIdAsync(model.Category.Id);

            category.Name = model.Category.Name;
            category.Description = model.Category.Description;

            if (model.SelectedProducts != null)
            {
                foreach (var item in model.SelectedProducts)
                {
                    var products = new ProductToCategory()
                    {
                        CategoryId = model.Category.Id,
                        ProductId = item
                    };
                    _context.Remove(products);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            var productToCategory = await _context.ProductToCategories.FirstOrDefaultAsync(c => c.CategoryId == category.Id);

            _context.Remove(category);

            if (productToCategory != null)
                _context.Remove(productToCategory);

            await _context.SaveChangesAsync();
        }
    }
}