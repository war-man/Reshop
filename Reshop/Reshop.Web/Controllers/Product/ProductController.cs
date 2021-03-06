﻿#nullable enable
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Reshop.Application.Services;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Models.User.Comment;
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
            return View(await _uow.ProductRe.GetProductsWithPagingAsync());
        }

        [Authorize]
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

        [HttpGet]
        [Route("ChildCategory/{childCategoryId}/{childCategoryName}")]
        public async Task<IActionResult> ShowProductsByChildCategoryId(int childCategoryId, string childCategoryName)
        {
            ViewData["ChildCategoryName"] = childCategoryName;

            return View(await _uow.ProductRe.ShowProductsByChildCategoryId(childCategoryId));
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
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddCommentToProduct", model) });

            try
            {
                await _uow.ProductRe.AddCommentToProduct(model);

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Product/_CommentsOfProduct", _uow.ProductRe.GetCommentsOfProduct(model.ProductId)) });
            }
            catch (Exception)
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddCommentToProduct", model) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerToComment(AnswerToComment model)
        {
            await _uow.ProductRe.AnswerToComment(model);

            return RedirectToAction(nameof(Index));
        }


        //--------------------------------- json ---------------------------------

        [HttpGet]
        public async Task<IActionResult> GetProductsByPage(int pageId)
        {
            return Json(new { html = Helper.RenderRazorViewToString(this, "Product/_BoxOfProducts", await _uow.ProductRe.GetProductsWithPagingAsync(pageId)) });
        }

        public async Task<IActionResult> SearchProductByFilter(string productName)
        {
            ViewData["productName"] = productName;

            return Json(string.IsNullOrEmpty(productName) ? new { html = Helper.RenderRazorViewToString(this, "Product/_BoxOfProducts", await _uow.ProductRe.GetProductsWithPagingAsync()) } : new { html = Helper.RenderRazorViewToString(this, "Product/_BoxOfProducts", await _uow.ProductRe.SearchProductByFilterAsync(productName)) });
        }

        public async Task<IActionResult> GetCommentsOfProduct(int productId, int take = 20)
        {
            return Json(new { html = Helper.RenderRazorViewToString(this, "Product/_CommentsOfProduct", await _uow.ProductRe.GetCommentsOfProduct(productId, take)) });
        }
    }
}
