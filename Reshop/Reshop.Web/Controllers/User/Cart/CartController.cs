using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Domain.Services.Interfaces;
using Reshop.Infrastructure.Context;

namespace Reshop.Web.Controllers.User.Cart
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CartController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int itemId,string userId)
        {
            await _uow.CartRe.AddToCartAsync(itemId, userId);

            var product = await _uow.ProductRe.FindByIdAsync(itemId);

            if (product == null) return NotFound();

            Uri uri = new Uri("https://localhost:44381" + $"/Product/{product.Id}/{product.Name.Replace(" ", "-")}");

            return Redirect(uri.AbsoluteUri);
        }

        [HttpGet]
        public async Task<IActionResult> ShowCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(await _uow.CartRe.ShowCartAsync(userId));
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Route("Card/Remove/{detailId}")]
        public async Task<IActionResult> RemoveCart(int detailId)
        {
            await _uow.CartRe.RemoveCartAsync(detailId);

            return Json(new {success = true, message = "delete successful"});
        }
    }
}
