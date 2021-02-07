using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Domain.Services.Interfaces;

namespace Reshop.Web.Components
{
    public class ProductCommentsViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _uow;

        public ProductCommentsViewComponent(IUnitOfWork uow)
        {
            _uow = uow;
        }



        public async Task<IViewComponentResult> InvokeAsync(int productId, int take = 20)
        {
            return View("/Views/Shared/Product/_CommentsOfProduct.cshtml", await _uow.ProductRe.GetCommentsForProduct(productId));
        }
    }
}