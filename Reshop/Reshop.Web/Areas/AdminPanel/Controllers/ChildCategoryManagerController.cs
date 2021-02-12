using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Services;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Services.Interfaces;

namespace Reshop.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ChildCategoryManagerController : Controller
    {
        private readonly IUnitOfWork _uow;

        public ChildCategoryManagerController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet]
        public async Task<IActionResult> GetChildCategories() => View(await _uow.CategoryRe.GetChildCategoriesAsync());

        [HttpGet]
        public async Task<IActionResult> AddOrEditChildCategory(int childCategoryId = 0)
        {
            return childCategoryId == 0 ? View() : View(await _uow.CategoryRe.GetChildCategoryColumnsAsync(childCategoryId));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditChildCategory(ChildCategory model)
        {

            try
            {
                if (model.Id == 0)
                {
                    await _uow.CategoryRe.AddChildCategoryAsync(model);
                }
                else
                {
                    await _uow.CategoryRe.EditChildCategoryAsync(model);
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Category/_BoxManageChildCategories", await _uow.CategoryRe.GetChildCategoriesAsync()) });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}