using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reshop.Application.Services;
using Reshop.Domain.Services.Interfaces;
using Reshop.Domain.ViewModels.ProductAndCategory.Category;
using Reshop.Domain.ViewModels.ProductAndCategory.Product;

namespace Reshop.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductsManagerController : Controller
    {
        private readonly IUnitOfWork _uow;

        public ProductsManagerController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int pageId = 1)
        {
            return View(await _uow.ProductRe.GetProductsWithPagingAsync(pageId));
        }

        #region add or edit product

        [HttpGet]
        [Helper.NoDirectAccessAttribute]
        public async Task<IActionResult> AddOrEditProduct(int productId = 0)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return NotFound();

            try
            {
                return productId == 0 ? View(await _uow.CategoryRe.GetAllCategoriesForAddingProduct(userId)) : View(await _uow.ProductRe.GetProductColumnsForEditAsync(productId, userId));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Helper.NoDirectAccessAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditProduct(AddOrEditProductViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditProduct", model) });

            try
            {
                if (model.Id == 0)
                {
                    await _uow.ProductRe.AddProductAsync(model);
                }
                else
                {
                    await _uow.ProductRe.EditProductAsync(model);
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Product/_BoxManageProducts", await _uow.ProductRe.GetProductsWithPagingAsync()) });
            }

            catch (DbUpdateConcurrencyException)
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditProduct", model) });
            }
        }

        #endregion



        #region AddCategoryToProductCategories

        [HttpGet]
        public async Task<IActionResult> AddCategoryToProductCategories(int productId)
        {
            return View(await _uow.CategoryRe.GetCategoriesThatProductDonotHaveAsync(productId));
        }

        [HttpPost]
        [Helper.NoDirectAccessAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategoryToProductCategories(AddCategoryToProductCategories model)
        {
            if (!ModelState.IsValid) return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddCategoryToProductCategories", model) });

            await _uow.CategoryRe.AddCategoryToProductCategoriesAsync(model);

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Product/_BoxManageProducts", await _uow.ProductRe.GetProductsWithPagingAsync()) });
        }

        #endregion

        #region Delete

        [HttpPost]
        [Helper.NoDirectAccessAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return NotFound();

            await _uow.ProductRe.DeleteProductAsync(productId, userId);

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Product/_BoxManageProducts", await _uow.ProductRe.GetProductsWithPagingAsync()) });
        }

        #endregion
    }
}