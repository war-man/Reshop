using System;
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
        {
            return await _context.Categories
                .Select(c => new ShowCategoryViewModel()
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    ChildCategories = _context.ChildCategoryToCategories.Where(g => g.CategoryId == c.Id)
                        .Select(c => c.ChildCategory).ToList(), // get child categories of category
                }).ToListAsync();
        }

        public async Task<AddCategoryToProductCategories> GetCategoriesThatProductDonotHaveAsync(int id)
        {
            var productCategories = await _context.Products
                .Where(c => c.Id == id)
                .SelectMany(c => c.ProductToCategories)
                .Select(ca => ca.Category).ToListAsync();

            var categories = await _context.Categories
                .Where(c => !productCategories.Contains(c))
                .ToListAsync();

            var productChildCategories = await _context.Products
                .Where(c => c.Id == id)
                .SelectMany(c => c.ProductToChildCategories)
                .Select(c => c.ChildCategory).ToListAsync();

            var childCategories = await _context.ChildCategories
                .Where(c => !productChildCategories.Contains(c)).ToListAsync();

            return new AddCategoryToProductCategories()
            {
                ProductId = id,
                Categories = categories,
                ChildCategories = childCategories
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

            foreach (var child in model.SelectedChildCategories)
            {
                var productToChildCategory = new ProductToChildCategory()
                {
                    ProductId = model.ProductId,
                    ChildCategoryId = child
                };
                await _context.AddAsync(productToChildCategory);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddCategoryAsync(AddOrEditCategoryViewModel model)
        {
            var ca = new Category()
            {
                Name = model.CategoryName,
                Description = model.CategoryDescription
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
                CategoryId = categoryId,
                CategoryName = category.Name,
                CategoryDescription = category.Description,
                Products = products
            };
        }

        public async Task<Category> FindCategoryByIdAsync(int categoryId)
        =>
            await _context.Categories.FindAsync(categoryId);

        public async Task EditCategoryAsync(AddOrEditCategoryViewModel model)
        {
            var category = await _context.Categories.FindAsync(model.CategoryId);

            category.Name = model.CategoryName;
            category.Description = model.CategoryDescription;

            if (model.SelectedProducts != null)
            {
                foreach (var item in model.SelectedProducts)
                {
                    var products = new ProductToCategory()
                    {
                        CategoryId = model.CategoryId,
                        ProductId = item
                    };
                    _context.Remove(products);
                }
            }

            await _context.SaveChangesAsync();
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

        // ------------- child category -------------

        public async Task<IEnumerable<ChildCategory>> GetChildCategoriesAsync()
        {
            return await _context.ChildCategories.ToListAsync();
        }

        public async Task EditChildCategoryAsync(ChildCategory model)
        {
            var childCategory = await _context.ChildCategories.FindAsync(model.Id);

            try
            {
                childCategory.Name = model.Name;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task AddChildCategoryAsync(ChildCategory model)
        {
            try
            {
                var childCategory = new ChildCategory()
                {
                    Name = model.Name
                };

                await _context.ChildCategories.AddAsync(childCategory);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ChildCategory> GetChildCategoryColumnsAsync(int childCategoryId)
        {
            var childCategory = await _context.ChildCategories.FindAsync(childCategoryId);

            return new ChildCategory()
            {
                Id = childCategory.Id,
                Name = childCategory.Name
            };
        }

        public async Task<AddChildCategoryToCategoryViewModel> GetChildCategoriesThatCategoryDonotHaveAsync(int categoryId)
        {
            var childCategoriesOfCategory = await _context.Categories
                .Where(c => c.Id == categoryId)
                .SelectMany(c => c.ChildCategoryToCategories)
                .Select(c => c.ChildCategory).ToListAsync();

            var childCategories = await _context.ChildCategories
                .Where(c => !childCategoriesOfCategory.Contains(c))
                .ToListAsync();

            return new AddChildCategoryToCategoryViewModel()
            {
                CategoryId = categoryId,
                ChildCategories = childCategories
            };
        }

        public async Task AddChildCategoryToCategory(AddChildCategoryToCategoryViewModel model)
        {
            foreach (var productToCategory in model.SelectedChildCategories
                .Select(i => new ChildCategoryToCategory()
                {
                    CategoryId = model.CategoryId,
                    ChildCategoryId = i
                }))
            {
                await _context.AddAsync(productToCategory);
            }

            await _context.SaveChangesAsync();
        }
    }
}