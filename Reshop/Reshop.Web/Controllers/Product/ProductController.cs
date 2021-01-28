#nullable enable
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Services.Interfaces;

namespace Reshop.Web.Controllers.Product
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uow;

        public ProductController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageId = 1)
        {
            return View(await _uow.ProductRe.GetAllProductsAsync(pageId));
        }

        [Authorize]
        [HttpGet]
        [Route("Product/{productId}/{productName}")]
        public async Task<IActionResult> DetailOfProduct(int productId, string? productName)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(await _uow.ProductRe.GetDetailOfProductAsync(productId, userId));
        }

        [HttpGet]
        [Route("Category/{categoryId}/{categoryName}")]
        public async Task<IActionResult> ShowProductsByCategoryId(int categoryId, string categoryName)
        {
            ViewData["CategoryName"] = categoryName;

            return View(await _uow.ProductRe.ShowProductsByCategoryIdAsync(categoryId));
        }

        [HttpGet]
        [Route("p/{key}")]
        public async Task<IActionResult> ShortKeyRedirect(string key)
        {
            var product = await _uow.ProductRe.FindProductByShortKeyAsync(key);

            if (product == null) return NotFound();

            Uri uri = new Uri("https://localhost:44381" + $"/Product/{product.Id}/{product.Name.Replace(" ", "-")}");

            return Redirect(uri.AbsoluteUri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCommentToProduct(CommentForProduct model)
        {

            if (ModelState.IsValid)
            {
                await _uow.ProductRe.AddCommentToProduct(model);
            }
            else
            {
                return NotFound();
            }

            var product = await _uow.ProductRe.FindByIdAsync(model.ProductId);
            if (product == null) return NotFound();

            Uri uri = new Uri("https://localhost:44381" + $"/Product/{product.Id}/{product.Name.Replace(" ", "-")}");

            return Redirect(uri.AbsoluteUri);
        }

    }
}
