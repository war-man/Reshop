using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Reshop.Application.Services.ExtensionMethods;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Services.Interfaces.ProductAndCategory;
using Reshop.Domain.ViewModels.ProductAndCategory.Category;
using Reshop.Domain.ViewModels.ProductAndCategory.Product;
using Reshop.Infrastructure.Context;

namespace Reshop.Infrastructure.Services.Repository.ProductAndCategory
{
    public class ProductRe : IProductRe
    {
        private readonly ReshopDbContext _context;

        public ProductRe(ReshopDbContext context)
        {
            _context = context;
        }

        public async Task<ShowProductsViewModel> GetProductsWithPagingAsync(int pageId = 1, int take = 4)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _context.Products.CountAsync();

            double totalPages = Math.Ceiling(1.0 * productsCount / take); // get count of pages


            var products = await _context.Products.Include(c => c.Item).OrderBy(c => c.Id)
                .Skip(skip).Take(take).ToListAsync();

            return new ShowProductsViewModel()
            {
                Products = products,
                PageCount = totalPages,
                PageId = pageId
            };
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<DetailViewModel> GetDetailOfProductAsync(int productId, string userId)
        {
            // get selected product by its item
            var product = await _context.Products
                .Include(c => c.Item)
                .FirstOrDefaultAsync(c => c.Id == productId);

            // get product categories
            var categoryOfProduct = await _context.Products
                .Where(c => c.Id == productId)
                .SelectMany(c => c.ProductToCategories)
                .Select(c => c.Category)
                .ToListAsync();

            return new DetailViewModel()
            {
                UserId = userId,
                Product = product,
                Categories = categoryOfProduct,
            };
        }

        public async Task<ShowProductsViewModel> ShowProductsByCategoryIdAsync(int categoryId, int take = 20, int pageId = 1)
        {
            int skip = (pageId - 1) * take;

            int productsCount = await _context.ProductToCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.Product).CountAsync();

            double totalPages = Math.Ceiling(1.0 * productsCount / take); // get count of pages




            var products = await _context.ProductToCategories
                .Where(c => c.CategoryId == categoryId)
                .Include(c => c.Product)
                .ThenInclude(c => c.Item)
                .Select(c => c.Product)
                .OrderByDescending(c => c.Id)
                .Skip(skip).Take(take).ToListAsync();

            return new ShowProductsViewModel()
            {
                Products = products,
                PageId = pageId,
                PageCount = totalPages
            };
        }

        public async Task<ShowProductsViewModel> ShowProductsByChildCategoryId(int childCategoryId, int take = 20, int pageId = 1)
        {
            int skip = (pageId - 1) * take;

            int productsCount = await _context.ProductToChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.Product).CountAsync();

            double totalPages = Math.Ceiling(1.0 * productsCount / take); // get count of pages

            var products = await _context.ProductToChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .Include(c => c.Product)
                .ThenInclude(c => c.Item)
                .Select(c => c.Product)
                .OrderByDescending(c=> c.Id)
                .Skip(skip).Take(take).ToListAsync();

            return new ShowProductsViewModel()
            {
                Products = products,
                PageId = pageId,
                PageCount = totalPages
            };

        }

        public async Task<ShowProductsViewModel> SearchProductByFilterAsync(string productName)
        {
            var product = await _context.Products
                .Include(c => c.Item)
                .Where(c => c.Name.Contains(productName))
                .AsNoTracking()
                .ToListAsync();
            return new ShowProductsViewModel()
            {
                Products = product
            };
        }

        public async Task<AddOrEditProductViewModel> GetProductColumnsForEditAsync(int productId, string userId)
        {
            var categories = await _context.Products
                .Where(c => c.Id == productId)
                .SelectMany(c => c.ProductToCategories)
                .Select(c => new Category()
                {
                    Id = c.Category.Id,
                    Name = c.Category.Name
                }).ToListAsync();

            return await _context.Products
                .Include(p => p.Item)
                .Where(p => p.Id == productId)
                .Select(s => new AddOrEditProductViewModel()
                {
                    UserId = userId,
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ShortKey = s.ShortKey,
                    QuantityInStock = s.Item.QuantityInStock,
                    Price = s.Item.Price,
                    Categories = categories
                }).FirstOrDefaultAsync();
        }

        public async Task EditProductAsync(AddOrEditProductViewModel model)
        {
            var pro = await _context.Products.FindAsync(model.Id);

            // get product item
            var item = await _context.Items.FirstAsync(c => c.Id == pro.ItemId);


            pro.Name = model.Name;
            pro.Description = model.Description;
            item.Price = model.Price;
            item.QuantityInStock = model.QuantityInStock;


            // add category to product
            if (model.SelectedCategories != null)
            {
                foreach (var productToCategory in model.SelectedCategories
                    .Select(t => new ProductToCategory()
                    {
                        ProductId = pro.Id,
                        CategoryId = t
                    }))
                {
                    _context.Remove(productToCategory);
                }
            }

            await _context.SaveChangesAsync();

            // add picture to product
            if (model.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    pro.ImageId + Path.GetExtension(model.Picture.FileName));


                await using var stream = new FileStream(filePath, FileMode.Create);
                await model.Picture.CopyToAsync(stream);
            }
        }

        public async Task AddProductAsync(AddOrEditProductViewModel model)
        {
            var item = new Item()
            {
                Price = model.Price,
                QuantityInStock = model.QuantityInStock
            };

            await _context.AddAsync(item);
            await _context.SaveChangesAsync();


            var pro = new Product()
            {
                Name = model.Name,
                Item = item,
                Description = model.Description,
                ShortKey = GenerateShortKey(),
                DateTime = DateTime.Now.ToShamsi()
            };

            await _context.AddAsync(pro);
            await _context.SaveChangesAsync();


            pro.ItemId = pro.Id;
            pro.ImageId = pro.Id;
            await _context.SaveChangesAsync();


            if (model.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    pro.ImageId + Path.GetExtension(model.Picture.FileName));

                await using var stream = new FileStream(filePath, FileMode.Create);
                await model.Picture.CopyToAsync(stream);
            }

            if (model.SelectedCategories != null)
            {
                foreach (var productToCategory in model.SelectedCategories
                    .Select(category => new ProductToCategory()
                    {
                        ProductId = pro.Id,
                        CategoryId = category
                    }))
                {
                    await _context.AddAsync(productToCategory);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int productId, string userId)
        {
            var product = await _context.Products.FindAsync(productId);
            var item = _context.Items.First(p => p.Id == product.ItemId);
            var productToCategory = _context.ProductToCategories.FirstOrDefault(c => c.ProductId == product.Id);

            _context.Items.Remove(item);
            _context.Products.Remove(product);


            if (productToCategory != null)
            {
                _context.ProductToCategories.Remove(productToCategory);
            }

            await _context.SaveChangesAsync();

            // delete product picture
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                product.Id + ".jpg");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<Product> FindByIdAsync(int productId)
        =>
            await _context.Products.FirstOrDefaultAsync(c => c.Id == productId);

        public async Task<Product> FindProductByShortKeyAsync(string key)
        =>
            await _context.Products.FirstOrDefaultAsync(c => c.ShortKey == key);

        public async Task<AddOrEditCategoryViewModel> GetAllProductsForAddingCategory()
        {
            var products = await _context.Products
                .Select(c => new Product()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();

            return new AddOrEditCategoryViewModel()
            {
                Products = products
            };
        }

        public async Task AddCommentToProduct(CommentForProduct model)
        {
            var comment = new CommentForProduct()
            {
                Comment = model.Comment,
                UserId = model.UserId,
                FullName = model.FullName,
                ProductId = model.ProductId,
                DateTime = model.DateTime
            };
            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentForProduct>> GetCommentsOfProduct(int productId, int take = 20)
        {
            return await _context.Products
                .Where(c => c.Id == productId)
                .SelectMany(c => c.CommentsForProduct)
                .OrderByDescending(c => c.Comment).Take(take).ToListAsync();

        }


        #region GenerateShortKey

        private string GenerateShortKey(int Length = 3)
        {
            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, Length);
            while (_context.Products.Any(a => a.ShortKey == key))
            {
                key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, Length);
            }

            return key;
        }

        #endregion
    }
}

