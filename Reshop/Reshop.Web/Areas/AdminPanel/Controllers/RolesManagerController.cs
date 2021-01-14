using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Models.User.Identity;
using Reshop.Domain.ViewModels.User.Role;

namespace Reshop.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class RolesManagerController : Controller
    {
        private readonly RoleManager<Role> _roleManager;

        public RolesManagerController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }


        // Show All roles
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles
                .Select(c => new RoleViewModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name
                }).ToListAsync();

            return View(roles);
        }

        #region Add Role

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            // We Take A Role Name 
            // Show 404 Error When Name Be Empty
            if (!ModelState.IsValid) return View(model);

            // Add Role To IdentityRole
            var role = new Role()
            {
                Name = model.Name
            };

            // Create Role
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            // Show Errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        #endregion

        #region Edit Role

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            // Show 404 Error When Id is Empty
            if (string.IsNullOrEmpty(roleId)) return NotFound();

            // Find Role Id
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            var model = new RoleViewModel()
            {
                Id = role.Id.ToString(),
                Name = role.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RoleViewModel model)
        {
            // Show Error When Role Id And Role Name Are Empty
            if (model == null) return NotFound();

            // Find Role Id
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null) return NotFound();

            // Change Role Name And Put New Role Name
            role.Name = model.Name;

            // Update Role
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded) return RedirectToAction(nameof(Index));

            // Show Errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }


            return View(model);
        }

        #endregion

        #region Delete 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            if (roleId == null) return NotFound();

            //Find Role Id
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) return NotFound();

            //Delete Role
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded) return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}