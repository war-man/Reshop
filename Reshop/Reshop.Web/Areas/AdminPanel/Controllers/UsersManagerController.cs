using Microsoft.AspNetCore.Mvc;

namespace Reshop.Web.Areas.AdminPanel.Controllers
{
    public class UsersManagerController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}