using Microsoft.AspNetCore.Mvc;

namespace Reshop.Web.Areas.AdminPanel.Controllers
{
    public class RolesManagerController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}