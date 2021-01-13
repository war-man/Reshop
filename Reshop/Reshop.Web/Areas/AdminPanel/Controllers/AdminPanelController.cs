using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Reshop.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminPanelController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}