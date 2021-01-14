using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            return View(await _uow.ProductRe.GetAllProductsAsync());
        }

        #region Add

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return NotFound();

            return View(await _uow.CategoryRe.GetAllCategoriesForAddingProduct(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddOrEditProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _uow.ProductRe.AddProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> EditProduct(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return NotFound();

            return View(await _uow.ProductRe.GetProductColumnsForEditProductAsync(productId,userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(AddOrEditProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _uow.ProductRe.EditProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region AddCategoryToProductCategories

        [HttpGet]
        public async Task<IActionResult> AddCategoryToProductCategories(int productId)
        {
            return View(await _uow.CategoryRe.GetCategoriesThatProductDonotHaveAsync(productId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategoryToProductCategories(AddCategoryToProductCategories model)
        {
            if (!ModelState.IsValid) return View(model);

            await _uow.CategoryRe.AddCategoryToProductCategoriesAsync(model);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return NotFound();

            await _uow.ProductRe.DeleteProductAsync(productId, userId);

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}