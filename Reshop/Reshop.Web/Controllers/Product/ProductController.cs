using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            return View(await _uow.ProductRe.GetAllProductsAsync());
        }

        [HttpGet]
        [Route("Product/{productId}/{productName}")]
        public async Task<IActionResult> DetailOfProduct(int productId, string productName)
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
    }
}
