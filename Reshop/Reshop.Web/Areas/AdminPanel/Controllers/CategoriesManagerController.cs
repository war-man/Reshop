using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddCategory()
        {
            return View(await _uow.ProductRe.GetAllProductsForAddingCategory());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddOrEditCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _uow.CategoryRe.AddCategoryAsync(model);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> EditCategory(int categoryId)
        {
            return View(await _uow.CategoryRe.GetCategoryColumnsWithItsProductsAsync(categoryId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(AddOrEditCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _uow.CategoryRe.EditCategoryAsync(model);

            return RedirectToAction(nameof(Index));
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
    }
}