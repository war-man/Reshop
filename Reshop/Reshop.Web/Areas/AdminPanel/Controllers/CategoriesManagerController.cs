using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reshop.Application.Services;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Services.Interfaces;
using Reshop.Domain.ViewModels.ProductAndCategory.Category;

namespace Reshop.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoriesManagerController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CategoriesManagerController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _uow.CategoryRe.GetAllCategories());
        }

        #region Add


        [HttpGet]
        public async Task<IActionResult> AddOrEditCategory(int categoryId = 0)
        {
            return categoryId == 0 ? View(await _uow.ProductRe.GetAllProductsForAddingCategory()) : View(await _uow.CategoryRe.GetCategoryColumnsWithItsProductsAsync(categoryId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditCategory(AddOrEditCategoryViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditCategory", model) });
            try
            {
                if (model.CategoryId == 0)
                {
                    await _uow.CategoryRe.AddCategoryAsync(model);
                }
                else
                {
                    await _uow.CategoryRe.EditCategoryAsync(model);
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Category/_BoxManageCategories", await _uow.CategoryRe.GetAllCategories()) });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditCategory", model) });
            }
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _uow.CategoryRe.DeleteCategoryAsync(categoryId);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> AddChildCategoryToCategory(int categoryId)
        {
            return View(await _uow.CategoryRe.GetChildCategoriesThatCategoryDonotHaveAsync(categoryId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChildCategoryToCategory(AddChildCategoryToCategoryViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddChildCategoryToCategory", model) });

            await _uow.CategoryRe.AddChildCategoryToCategory(model);

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Category/_BoxManageCategories", await _uow.CategoryRe.GetAllCategories()) });
        }
    }
}